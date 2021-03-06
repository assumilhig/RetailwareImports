CREATE PROCEDURE [dbo].[Proc_Import_NewItems] @FilePath nvarchar(500), @Server nvarchar(50), @Database nvarchar(50), @UserName nvarchar(50), @Password nvarchar(50)
AS
    IF OBJECT_ID('dbo.tmpImportNewItems', 'U') IS NOT NULL DROP TABLE dbo.tmpImportNewItems

    CREATE TABLE [dbo].tmpImportNewItems(
        [Itemcode] [nvarchar](100) NULL,
        [Description] [nvarchar](500) NULL,
        [ExtendedDesc] [nvarchar](500) NULL,
        [Department] [nvarchar](500) NULL,
        [Category] [nvarchar](250) NULL,
        [SubCategory] [nvarchar](250) NULL,
        [Price] [money] NULL,
        [PriceA] [money] NULL,
        [PriceB] [money] NULL,
        [PriceC] [money] NULL,
		[SCFlag] [varchar](3) NULL,
        [RUOM] [nvarchar](100) NULL,
        [Taxable] [varchar](3) NULL,
        [PackingQty] [float] NULL,
        [WUOM] [nvarchar](100) NULL,
    ) ON [PRIMARY]

	DECLARE @str nvarchar(1000)

	SET @str = 'bcp [' + @Database + ']..tmpImportNewItems in "' + @FilePath + '" -b2000 -c -t~~ -U' + @UserName + ' /' + 'P' + @Password + ' /S' + @Server + ' /c /t,'
	EXEC master..xp_cmdshell @str

    DELETE FROM tmpImportNewItems WHERE LTRIM(RTRIM(ItemCode)) = 'ItemCode'

    IF OBJECT_ID('dbo.ImportNewItems', 'U') IS NOT NULL DROP TABLE dbo.ImportNewItems

    SELECT ROW_NUMBER() OVER (ORDER BY ItemCode ASC) As ID, *, NULL AS DepartmentID, NULL AS CategoryID, NULL AS SubCategoryID INTO ImportNewItems FROM tmpImportNewItems
	
    IF OBJECT_ID('dbo.tmpImportNewItems', 'U') IS NOT NULL DROP TABLE dbo.tmpImportNewItems