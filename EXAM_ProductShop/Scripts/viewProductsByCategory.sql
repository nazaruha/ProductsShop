SELECT p.Id, p.[Name] AS Product, p.Price, c.[Name] AS Category, sc.[Name] AS [Sub-Category]
FROM tblProducts AS p
INNER JOIN tblCategories AS c
ON p.CategoryId = c.Id
INNER JOIN tblSubCategories AS sc
ON p.SubCategoryId = sc.Id
WHERE p.CategoryId = @IdField