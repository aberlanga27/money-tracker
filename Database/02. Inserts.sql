INSERT INTO [Catalogue].[TransactionCategory] (TransactionCategoryName, TransactionCategoryDescription, TransactionCategoryIcon, TransactionCategoryColor)
VALUES
    ('Food', 'Food and Groceries', 'fa-utensils', 'FF0F00'),
    ('Transport', 'Transportation and Fuel', 'fa-car', 'F0FF00'),
    ('Utilities', 'Utilities and Bills', 'fa-bolt', 'F000FF'),
    ('Entertainment', 'Entertainment and Leisure', 'fa-gamepad', 'FFFF0F'),
    ('Health', 'Health and Fitness', 'fa-heart', 'FF00FF'),
    ('Shopping', 'Shopping and Clothing', 'fa-shopping-bag', '00FFFF'),
    ('Travel', 'Travel and Vacation', 'fa-plane', 'FF800F'),
    ('Education', 'Education and Books', 'fa-book', 'F0F0FF'),
    ('Gifts', 'Gifts and Donations', 'fa-gift', '0080FF'),
    ('Investment', 'Investment and Savings', 'fa-piggy-bank', 'FF0080'),
    ('Other', 'Other and Miscellaneous', 'fa-question', 'FF8000')

INSERT INTO [Catalogue].[Bank] (BankName)
VALUES
    ('Banorte'),
    ('Banamex'),
    ('Rappi'),
    ('Nu'),
    ('Mercado Pago'),
    ('Uala'),
    ('GBM'),
    ('Finsus'),
    ('Hey Banco')

INSERT INTO [Catalogue].[TransactionType] (TransactionTypeName, TransactionTypeDescription)
VALUES
    ('Income', 'Income and Salary'),
    ('Expense', 'Expense and Spending')

INSERT INTO [Catalogue].[BudgetType] (BudgetTypeName, BudgetTypeDays)
VALUES
    ('Daily', 1),
    ('Weekly', 7),
    ('Bi-Weekly', 14),
    ('Monthly', 30),
    ('Quarterly', 90),
    ('Yearly', 365)

INSERT INTO [Analytics].[Budget] (TransactionCategoryId, BudgetTypeId, BudgetAmount)
VALUES
    (1, 4, 1000.00),
    (2, 4, 500.00),
    (3, 4, 300.00),
    (4, 4, 200.00),
    (5, 4, 100.00),
    (6, 4, 200.00),
    (7, 4, 500.00),
    (8, 4, 300.00),
    (9, 4, 100.00),
    (10, 4, 200.00)
