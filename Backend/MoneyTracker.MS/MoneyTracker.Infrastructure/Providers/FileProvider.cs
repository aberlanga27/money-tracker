namespace MoneyTracker.Infrastructure.Providers;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MoneyTracker.Domain.Interfaces;
using Newtonsoft.Json;

[ExcludeFromCodeCoverage]
public class FileProvider() : IFileProvider
{
    private static readonly HashSet<Type> numericTypes =
    [
        typeof(int),
        typeof(long),
        typeof(short),
        typeof(byte),
        typeof(double),
        typeof(float),
        typeof(decimal)
    ];

    private static Dictionary<string, string> GetColumns<T>()
    {
        return typeof(T).GetProperties()
            .ToDictionary(
                p => p.Name,
                p => p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? p.Name
            );
    }

    private Row GetHeaderRow(List<string> records)
    {
        var headerRow = new Row();
        foreach (var column in records)
            headerRow.AppendChild(new Cell()
            {
                CellValue = new CellValue(column),
                DataType = CellValues.String
            });

        return headerRow;
    }

    public Row GetTitleRow(string title)
    {
        return new Row(new Cell()
        {
            CellValue = new CellValue(title),
            DataType = CellValues.String
        });
    }

    private static CellValues GetCellDataType(object? value)
    {
        if (value == null)
            return CellValues.String;

        var valueType = value.GetType();

        if (valueType == typeof(string))
            return CellValues.String;
        else if (numericTypes.Contains(valueType))
            return CellValues.Number;
        else if (valueType == typeof(bool))
            return CellValues.Boolean;
        else if (valueType == typeof(DateTime))
            return CellValues.Date;
        else
            return CellValues.String;
    }

    private Row GetFilledRow<T>(List<string> records, T item)
    {
        var row = new Row();
        foreach (var column in records)
        {
            var propertyInfo = typeof(T).GetProperty(column);
            var value = propertyInfo?.GetValue(item)?.ToString() ?? null;

            row.AppendChild(new Cell()
            {
                CellValue = (value != null)
                    ? new CellValue(value)
                    : new CellValue("-"),
                DataType = GetCellDataType(value)
            });
        }

        return row;
    }

    public MemoryStream CreateExcelFile<T>(string sheetName, List<T> data, string title = "")
    {
        var memoryStream = new MemoryStream();
        using var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);

        var workbookPart = document.AddWorkbookPart();
        var workbook = new Workbook();
        workbookPart.Workbook = workbook;

        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        var worksheet = new Worksheet(new SheetData());
        worksheetPart.Worksheet = worksheet;

        var sheets = workbook.AppendChild(new Sheets());
        sheets.AppendChild(new Sheet()
        {
            Id = workbookPart.GetIdOfPart(worksheetPart),
            SheetId = 1,
            Name = sheetName
        });

        var columns = GetColumns<T>();
        var headerRow = GetHeaderRow([.. columns.Values]);

        var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>()!;

        if (!string.IsNullOrEmpty(title))
            sheetData.AppendChild(GetTitleRow(title));
        sheetData.AppendChild(headerRow);

        foreach (var item in data)
        {
            var row = GetFilledRow([.. columns.Keys], item);
            sheetData.AppendChild(row);
        }

        workbook.Save();
        memoryStream.Position = 0;
        return memoryStream;
    }

    public MemoryStream AppendExcelFile<T>(MemoryStream baseMemoryStream, List<T> data, bool hasHeader = false, string title = "")
    {
        var memoryStream = new MemoryStream();
        memoryStream.Write(baseMemoryStream.ToArray(), 0, baseMemoryStream.ToArray().Length);

        using var document = SpreadsheetDocument.Open(memoryStream, true);
        var workbookPart = document.WorkbookPart;
        var worksheetPart = workbookPart?.WorksheetParts.First();
        var worksheet = worksheetPart?.Worksheet;
        var sheetData = worksheet?.GetFirstChild<SheetData>()!;

        var columns = GetColumns<T>();

        if (hasHeader)
        {
            var headerRow = GetHeaderRow([.. columns.Values]);
            sheetData.AppendChild(new Row());
            if (!string.IsNullOrEmpty(title))
                sheetData.AppendChild(GetTitleRow(title));
            sheetData.AppendChild(headerRow);
        }

        foreach (var item in data)
        {
            var row = GetFilledRow([.. columns.Keys], item);
            sheetData.AppendChild(row);
        }

        workbookPart?.Workbook.Save();
        memoryStream.Position = 0;
        return memoryStream;
    }
}