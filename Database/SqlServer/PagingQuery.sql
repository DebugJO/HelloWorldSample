--SQL Server 2012:
DECLARE @RowsPerPage INT = 10, @PageNumber INT = 6
SELECT SalesOrderDetailID, SalesOrderID, ProductID
FROM Sales.SalesOrderDetail
ORDER BY SalesOrderDetailID
OFFSET (@PageNumber-1)*@RowsPerPage ROWS
FETCH NEXT @RowsPerPage ROWS ONLY
GO

--SQL Server 2005/2008(R2):
DECLARE @RowsPerPage INT = 10, @PageNumber INT = 6
SELECT SalesOrderDetailID, SalesOrderID, ProductID
FROM (
SELECT SalesOrderDetailID, SalesOrderID, ProductID,
ROW_NUMBER() OVER (ORDER BY SalesOrderDetailID) AS RowNum
FROM Sales.SalesOrderDetail ) AS SOD
WHERE SOD.RowNum BETWEEN ((@PageNumber-1)*@RowsPerPage)+1
AND @RowsPerPage*(@PageNumber)
GO

SQL Server 2000:
DECLARE @RowsPerPage INT = 10, @PageNumber INT = 6
SELECT SalesOrderDetailID, SalesOrderID, ProductID
FROM
(
SELECT TOP (@RowsPerPage)
SalesOrderDetailID, SalesOrderID, ProductID
FROM
(
SELECT TOP ((@PageNumber)*@RowsPerPage)
SalesOrderDetailID, SalesOrderID, ProductID
FROM Sales.SalesOrderDetail
ORDER BY SalesOrderDetailID
) AS SOD
ORDER BY SalesOrderDetailID DESC
) AS SOD2
ORDER BY SalesOrderDetailID ASC
GO

--https://blog.sqlauthority.com/2013/04/14/sql-server-tricks-for-row-offset-and-paging-in-various-versions-of-sql-server/
