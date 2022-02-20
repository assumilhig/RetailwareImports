CREATE PROCEDURE [dbo].[Proc_Import_NewItemsValidate] @IsResto nvarchar(5)
AS 
    UPDATE a SET DepartmentID = b.ID FROM ImportNewItems a LEFT OUTER JOIN Department b on LTRIM(RTRIM(a.Department)) = LTRIM(RTRIM(b.Name))
    UPDATE a SET CategoryID = b.ID FROM ImportNewItems a LEFT OUTER JOIN Category b on a.DepartmentID = b.DepartmentID AND LTRIM(RTRIM(a.Category)) = LTRIM(RTRIM(b.Name))
    UPDATE a SET SubCategoryID = b.ID FROM ImportNewItems a LEFT OUTER JOIN SubCategory b on a.CategoryID = b.CategoryID AND LTRIM(RTRIM(a.SubCategory)) = LTRIM(RTRIM(b.Name))

	IF OBJECT_ID('dbo.ImportNewItemFindings', 'U') IS NOT NULL DROP TABLE dbo.ImportNewItemFindings
    CREATE TABLE [dbo].[ImportNewItemFindings](
        [Code] [nvarchar](100) NULL,
        [Validation] [nvarchar](100) NULL,
        [Result] [float] NULL,
		[Error] [int] NULL
    ) ON [PRIMARY]

    DECLARE @RecordCount int = 0
    DECLARE @MaxLength int = 0

	SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE Itemcode in (SELECT Itemcode from ImportNewItems GROUP BY Itemcode HAVING COUNT(Itemcode) > 1))
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('01', 'Duplicate Itemcode', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

	SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LTRIM(RTRIM(description)) in (SELECT LTRIM(RTRIM(description)) from ImportNewItems GROUP BY description HAVING COUNT(LTRIM(RTRIM(description))) > 1))
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('02', 'Duplicate description', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)
    
	SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE Itemcode in (SELECT Itemlookupcode FROM Item))
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('03', 'Itemcode with existing record', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

	SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(Itemcode,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('04', 'Blank Itemcode', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(Description,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('05', 'Blank Description', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(Department,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('06', 'Blank Department', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(Category,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('07', 'Blank Category', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(SubCategory,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('08', 'Blank SubCategory', @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE ISNULL(Price,0) = 0)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('09', 'Items with Zero Price', @RecordCount, 0)

    SET @MaxLength = (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Item' AND COLUMN_NAME = 'ItemLookupCode')
    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LEN(Itemcode) > @MaxLength)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('10', CONCAT('Itemcode exceeding ', @MaxLength,' Char'), @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @MaxLength = (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Item' AND COLUMN_NAME = 'Description')
    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LEN(Itemcode) > @MaxLength)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('11', CONCAT('Description exceeding ', @MaxLength,' Char'), @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @MaxLength = (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Department' AND COLUMN_NAME = 'Name')
    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LEN(Itemcode) > @MaxLength)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('12', CONCAT('Department exceeding ', @MaxLength,' Char'), @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @MaxLength = (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Category' AND COLUMN_NAME = 'Name')
    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LEN(Itemcode) > @MaxLength)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('13', CONCAT('Category exceeding ', @MaxLength,' Char'), @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @MaxLength = (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'SubCategory' AND COLUMN_NAME = 'Name')
    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LEN(Itemcode) > @MaxLength)
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('14', CONCAT('SubCategory exceeding ', @MaxLength,' Char'), @RecordCount, CASE WHEN @RecordCount > 0 THEN 1 ELSE 0 END)

    SET @RecordCount = (SELECT COUNT(*) FROM ImportNewItems WHERE LOWER(Taxable) <> 'yes' and LOWER(Taxable) <> 'no' OR ISNULL(Taxable,'') = '')
    INSERT INTO ImportNewItemFindings(Code, Validation,Result, Error) VALUES ('15', 'Invalid Tax Status', @RecordCount, 0)
