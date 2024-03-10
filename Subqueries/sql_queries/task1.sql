SELECT
    product.id,
    title AS product,
    product.price
FROM product_title
INNER JOIN product
    ON product_title.id = product.product_title_id
WHERE product.price >= 2 *
    (SELECT
        MIN(price)
    FROM product)
ORDER BY product.price, product