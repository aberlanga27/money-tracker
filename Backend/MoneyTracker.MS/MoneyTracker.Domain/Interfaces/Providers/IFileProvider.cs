namespace MoneyTracker.Domain.Interfaces;

public interface IFileProvider
{
    /// <summary>
    /// Create an Excel file from a list of objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheetName"></param>
    /// <param name="data"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    MemoryStream CreateExcelFile<T>(string sheetName, List<T> data, string title = "");

    /// <summary>
    /// Append data to an existing Excel file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="baseMemoryStream"></param>
    /// <param name="data"></param>
    /// <param name="hasHeader"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    MemoryStream AppendExcelFile<T>(MemoryStream baseMemoryStream, List<T> data, bool hasHeader = false, string title = "");
}