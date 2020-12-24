-- INSERT
DECLARE @NewRows TABLE(keycol INT, datacol NVARCHAR(40));

INSERT INTO dbo.T1(datacol)
	OUTPUT inserted.keycol, inserted.datacol
	INTO @NewRows
		SELECT lastname
		FROM HR.Employees
		WHERE country = N'UK';

SELECT * FROM @NewRows;

-- DELETE
DELETE FROM dbo.Orders
	OUTPUT
		deleted.orderid,
		deleted.orderdate,
		deleted.empid,
		deleted.custid
WHERE orderdate < '20080101';

-- UPDATE
UPDATE dbo.OrderDetails
	SET discount += 0.05
OUTPUT
	inserted.productid,
	deleted.discount AS olddiscount,
	inserted.discount AS newdiscount
WHERE productid = 51;

-- MERGE
MERGE INTO dbo.Customers AS TGT
USING dbo.CustomersStage AS SRC
	ON TGT.custid = SRC.custid
WHEN MATCHED THEN
	UPDATE SET
		TGT.companyname = SRC.companyname,
		TGT.phone = SRC.phone,
		TGT.address = SRC.address
WHEN NOT MATCHED THEN
	INSERT (custid, companyname, phone, address)
	VALUES (SRC.custid, SRC.companyname, SRC.phone, SRC.address)
OUTPUT $action AS theaction, inserted.custid,
	deleted.companyname AS oldcompanyname,
	inserted.companyname AS newcompanyname,
	deleted.phone AS oldphone,
	inserted.phone AS newphone,
	deleted.address AS oldaddress,
	inserted.address AS newaddress;

-- composable DML
INSERT INTO dbo.ProductsAudit(productid, colname, oldval, newval)
	SELECT productid, N'unitprice', oldval, newval
	FROM (UPDATE dbo.Products
		SET unitprice *= 1.15
	OUTPUT
		inserted.productid,
		deleted.unitprice AS oldval,
		inserted.unitprice AS newval
	WHERE supplierid = 1) AS D
WHERE oldval < 20.0 AND newval >= 20.0;
