USE [AdventureWorks2016]
/* Query 1 */
SELECT
    year(soh.[OrderDate]) as Year,
    p.[ProductID],
    ROUND(SUM(soh.[TotalDue]), 2) AS Total
FROM
    [Sales].[SalesOrderDetail] AS sd
    INNER JOIN [Sales].[SalesOrderHeader] AS soh ON sd.SalesOrderID = soh.SalesOrderID
    INNER JOIN [Production].[Product] as p ON p.ProductID = sd.ProductID
WHERE
    (
        year(soh.[OrderDate]) = 2011
        OR year(soh.[OrderDate]) = 2012
    )
    AND p.[ProductNumber] like 'FR-%'
    AND P.[Color] = 'black'
GROUP BY
    year(soh.[OrderDate]),
    p.[ProductID]
HAVING
    sum(soh.[TotalDue]) > 3000000
ORDER BY
    year(soh.[OrderDate]),
    p.[ProductID];

/* Query 2 */
SELECT
    year(soh.[OrderDate]) as Year,
    month(soh.[OrderDate]) as Month,
    DatePart(QUARTER, soh.[OrderDate]) as Quarter,
    ROUND(SUM(soh.[TotalDue]), 2) AS Total,
    SUM(ROUND(SUM(soh.[TotalDue]), 2)) OVER (
        PARTITION BY year(soh.[OrderDate])
        ORDER BY
            year(soh.[OrderDate]) ASC,
            month(soh.[OrderDate]) ASC
    ) AS AcumulatedTotal,
    SUM(ROUND(SUM(soh.[TotalDue]), 2)) OVER (
        PARTITION BY year(soh.[OrderDate]),
        DATEPART(quarter, soh.[OrderDate])
    ) AS QuarterTotal,
    ROUND(SUM(soh.[TotalDue]), 2) - LAG(ROUND(SUM(soh.[TotalDue]), 2), 1) OVER(
        ORDER BY
            year(soh.[OrderDate]),
            month(soh.[OrderDate]),
            DatePart(QUARTER, soh.[OrderDate])
    ) AS PreviousMonthDifference
FROM
    [Sales].[SalesOrderHeader] AS soh
GROUP BY
    year(soh.[OrderDate]),
    month(soh.[OrderDate]),
    DatePart(QUARTER, soh.[OrderDate])
ORDER BY
    year(soh.[OrderDate]),
    month(soh.[OrderDate]),
    DatePart(QUARTER, soh.[OrderDate]);

/* Query 3 
 Returns the first name and last name of the persons with their first address in alphabetical order, using `CROSS APPLY`.
 */
SELECT
    p.[FirstName],
    p.[LastName],
    dca.[AddressLine1]
FROM
    [Person].[Person] as p
    CROSS APPLY(
        SELECT
            TOP(1) [AddressLine1]
        FROM
            [Person].[Address] AS ad
            INNER JOIN [Person].[BusinessEntityAddress] as b ON p.BusinessEntityID = b.BusinessEntityID
        WHERE
            ad.AddressID = b.AddressID
    ) AS dca