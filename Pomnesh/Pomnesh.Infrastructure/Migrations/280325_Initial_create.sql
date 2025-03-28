-- Таблица для хранения типов вложений (если требуется, например, для перечисления AttachmentType)
CREATE TABLE AttachmentType (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- Таблица Users
CREATE TABLE Users (
    Id BIGINT PRIMARY KEY IDENTITY,
    VkId BIGINT NOT NULL,
    VkToken NVARCHAR(255) NULL
);

-- Таблица ChatContexts
CREATE TABLE ChatContexts (
    Id BIGINT PRIMARY KEY IDENTITY,
    MessageId BIGINT NOT NULL,
    MessageText NVARCHAR(255) NULL,
    MessageDate DATETIME NOT NULL
);

-- Таблица Recollections
CREATE TABLE Recollections (
    Id BIGINT PRIMARY KEY IDENTITY,
    UserId BIGINT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    DownloadLink NVARCHAR(255) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Таблица Attachments
CREATE TABLE Attachments (
    Id BIGINT PRIMARY KEY IDENTITY,
    Type INT NOT NULL,  -- Ссылка на тип AttachmentType
    FileId BIGINT NOT NULL,
    OwnerId BIGINT NOT NULL,
    OriginalLink NVARCHAR(255) NULL,
    ContextId BIGINT NOT NULL,  -- Ссылка на контекст
    FOREIGN KEY (OwnerId) REFERENCES Users(Id),
    FOREIGN KEY (ContextId) REFERENCES ChatContexts(Id),
    FOREIGN KEY (Type) REFERENCES AttachmentType(Id)
);

-- Индексы для оптимизации запросов
CREATE INDEX IX_AttachmentOwnerId ON Attachments (OwnerId);
CREATE INDEX IX_AttachmentContextId ON Attachments (ContextId);
CREATE INDEX IX_ChatContextMessageId ON ChatContexts (MessageId);
