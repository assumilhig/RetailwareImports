CREATE PROCEDURE [dbo].[Proc_Import_NewItemsProcess] @IsResto nvarchar(5)
AS
	DECLARE @MaxID int = 0

	SET @MaxID = (SELECT ISNULL(MAX(ID),0) FROM Department)
	ALTER TABLE Department ALTER COLUMN [Code] [nvarchar](50) NOT NULL;
	INSERT INTO Department SELECT NULL, LTRIM(RTRIM(Department)), @MaxID + ROW_NUMBER() OVER (ORDER BY Department ASC), 0, 1, NULL FROM ImportNewItems WHERE ISNULL(DepartmentID,'') = '' GROUP BY Department
	UPDATE a SET DepartmentID = b.ID FROM ImportNewItems a JOIN Department b on LTRIM(RTRIM(a.Department)) = LTRIM(RTRIM(b.Name))

	ALTER TABLE Category ALTER COLUMN [Code] [nvarchar](50) NOT NULL;
	INSERT INTO Category SELECT DepartmentID, LTRIM(RTRIM(Category)), CONCAT(DepartmentID,ROW_NUMBER() OVER(PARTITION BY DepartmentID ORDER BY Category ASC)), 0, 1, NULL, NULL FROM ImportNewItems WHERE ISNULL(CategoryID,'') = '' GROUP BY DepartmentID, Category
	UPDATE a SET CategoryID = b.ID FROM ImportNewItems a JOIN Category b on a.DepartmentID = b.DepartmentID AND LTRIM(RTRIM(a.Category)) = LTRIM(RTRIM(b.Name))

	ALTER TABLE SubCategory ALTER COLUMN [Code] [nvarchar](50) NOT NULL;
	INSERT INTO SubCategory SELECT CategoryID, LTRIM(RTRIM(SubCategory)), CONCAT(CategoryID,ROW_NUMBER() OVER(PARTITION BY CategoryID ORDER BY SubCategory ASC)), 0, 1, NULL FROM ImportNewItems WHERE ISNULL(SubCategoryID,'') = '' GROUP BY CategoryID, SubCategory
	UPDATE a SET SubCategoryID = b.ID FROM ImportNewItems a JOIN SubCategory b on a.CategoryID = b.CategoryID AND LTRIM(RTRIM(a.SubCategory)) = LTRIM(RTRIM(b.Name))

	INSERT INTO Item 
	SELECT 
		Itemcode, Description, ISNULL(ExtendedDesc,''), '' AS SizeCode, '' AS ColorCode,'' AS StyleCode,'' AS GroupItemcode, '' AS BinLocation, 0 AS BuydownPrice, 0 AS BuydownQuantity, 
		GETDATE() AS LastReceived, GETDATE() AS LastUpdated, '' as Notes, 0 as QuantityCommitted, 0 as GroupID, DepartmentID, CategoryID, SubCategoryID, CASE WHEN Taxable = 'Yes' THEN 1 ELSE 0 END AS MessageID, 
		ISNULL(Price,0), ISNULL(PriceA,0), ISNULL(PriceB,0), ISNULL(PriceC,0), 0 as SalePrice,
		GETDATE() AS SaleStartDate, GETDATE() AS SaleEndDate, 0, 0 as Cost, 0 as Quantity, 0 as ReorderPoint, 0 as RestockLevel, 0 as SupplierID, '' AS BarcodeFormat, 0 AS PriceLowerBound,
		0 AS PriceUpperBound, '' AS Picturename, GETDATE() as LastSold,'' as Brand, 'Regular Item' as ItemStatus, ISNULL(RUOM,''), ISNULL(WUOM,''), 0, 
		CASE WHEN Taxable = 'Yes' THEN 1 ELSE 0 END AS Taxable, 0 as LastCost, 0 as ReplacementCost, 0 AS Inactive, 0 AS LastCounted, 0 AS MSRP, GETDATE() AS DateCreated, '' AS ApplySalesCommission, '' AS ItemTag, ISNULL(PackingQty,1), '' AS Bin,
		'' AS ItemVendorGrp, 0 AS Sync, 0 AS HQ_Sync,(SELECT TOP 1 StoreCode FROM Configuration) AS StoreCode, 0 AS Freightrate, NULL as OrderNo 
	FROM ImportNewItems

	IF @IsResto = 'Yes'
	BEGIN
		DECLARE @MaxOrderNo int

		UPDATE a SET a.OrderNo = b.NewOrderNo FROM Department a JOIN (
			SELECT ID, OrderNo, ROW_NUMBER() OVER(ORDER BY OrderNo ASC) AS NewOrderNo FROM Department WHERE ISNULL(OrderNo,0) <> 0 AND ISNULL(OrderNo,'') <> ''
		) b ON a.ID = b.ID

		SET @MaxOrderNo = (SELECT ISNULL(MAX(OrderNo),0) FROM Department WHERE OrderNo <> 0 AND ISNULL(OrderNo,'') <> '')
		
		UPDATE a SET a.OrderNo = b.NewOrderNo FROM Department a JOIN (
			SELECT ID, OrderNo, @MaxOrderNo + ROW_NUMBER() OVER(ORDER BY Name ASC) AS NewOrderNo FROM Department WHERE ISNULL(OrderNo,'') = ''
		) b ON a.ID = b.ID

		DECLARE @DepartmentID int

		DECLARE db_cursor CURSOR FOR SELECT DepartmentID FROM Category GROUP BY DepartmentID

		OPEN db_cursor FETCH NEXT FROM db_cursor INTO @DepartmentID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE a SET a.OrderNo = b.NewOrderNo FROM Category a JOIN (
				SELECT ID, OrderNo, ROW_NUMBER() OVER(ORDER BY OrderNo ASC) AS NewOrderNo FROM Category WHERE OrderNo <> 0 AND ISNULL(OrderNo,'') <> '' AND DepartmentID = @DepartmentID
			) b ON a.ID = b.ID WHERE DepartmentID = @DepartmentID

			SET @MaxOrderNo = (SELECT ISNULL(MAX(OrderNo),0) FROM Category WHERE OrderNo <> 0 AND ISNULL(OrderNo,'') <> '' AND DepartmentID = @DepartmentID)

			UPDATE a SET a.OrderNo = b.NewOrderNo FROM Category a JOIN (
				SELECT ID, OrderNo, @MaxOrderNo + ROW_NUMBER() OVER(ORDER BY Name ASC) AS NewOrderNo FROM Category WHERE ISNULL(OrderNo,'') = '' AND DepartmentID = @DepartmentID
			) b ON a.ID = b.ID WHERE DepartmentID = @DepartmentID

			FETCH NEXT FROM db_cursor INTO @DepartmentID
		END
		CLOSE db_cursor
		DEALLOCATE db_cursor

		DECLARE @DepID int
		DECLARE @CatID int

		DECLARE db_cursor CURSOR FOR SELECT DepartmentID, CategoryID FROM Item GROUP BY DepartmentID, CategoryID

		OPEN db_cursor FETCH NEXT FROM db_cursor INTO @DepID, @CatID

		WHILE @@FETCH_STATUS = 0
		BEGIN

			UPDATE a SET a.OrderNo = b.NewOrderNo FROM Item a JOIN (
				SELECT ID, OrderNo, ROW_NUMBER() OVER(ORDER BY OrderNo ASC) AS NewOrderNo FROM Item WHERE OrderNo <> 0 AND ISNULL(OrderNo,'') <> '' AND DepartmentID = @DepID AND CategoryID = @CatID
			) b ON a.ID = b.ID WHERE DepartmentID = @DepID AND CategoryID = @CatID

			SET @MaxOrderNo = (SELECT ISNULL(MAX(OrderNo),0) FROM Item WHERE OrderNo <> 0 AND ISNULL(OrderNo,'') <> '' AND DepartmentID = @DepID AND CategoryID = @CatID)
	
			UPDATE a SET a.OrderNo = b.NewOrderNo FROM Item a JOIN (
				SELECT ID, OrderNo, @MaxOrderNo + ROW_NUMBER() OVER(ORDER BY OrderNo ASC) AS NewOrderNo FROM Item WHERE ISNULL(OrderNo,'') = '' AND DepartmentID = @DepID AND CategoryID = @CatID
			) b ON a.ID = b.ID WHERE DepartmentID = @DepID AND CategoryID = @CatID

			FETCH NEXT FROM db_cursor INTO @DepID, @CatID
		END
		CLOSE db_cursor
		DEALLOCATE db_cursor

		INSERT INTO ItemPrinter
		SELECT ItemLookupCode, '', '' FROM Item WHERE ItemLookupCode NOT IN (SELECT ItemLookupCode FROM ItemPrinter) 

		INSERT INTO ItemFlag
		SELECT ItemLookupCode, 1 FROM Item WHERE ItemLookupCode NOT IN (SELECT ItemLookupCode FROM ItemFlag) 
	END
