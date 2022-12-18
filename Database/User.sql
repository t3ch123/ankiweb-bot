CREATE TABLE IF NOT EXISTS "User" (
    ChatId serial PRIMARY KEY,
    Cookie VARCHAR(255) NOT NULL,
    CsrfToken VARCHAR(255) NOT NULL,
    State INT NOT NULL
)