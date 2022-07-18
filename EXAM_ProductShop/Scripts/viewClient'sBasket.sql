SELECT p.Id, p.[Name] AS Product, p.Price, b.[Count]
FROM tblBasket AS b
INNER JOIN tblProducts AS p
ON b.ProductId = p.Id
WHERE ClientId = @ClientIdField