CREATE TABLE IF NOT EXISTS "Users" (
    ChatId serial PRIMARY KEY,
    Cookie VARCHAR(255) NOT NULL,
    State INT NOT NULL
)