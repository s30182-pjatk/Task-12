-- Creating an authors table
CREATE TABLE IF NOT EXISTS Author (
    AuthorID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50)
);

-- Creating a table of book genres
CREATE TABLE IF NOT EXISTS Genre (
    GenreID INT AUTO_INCREMENT PRIMARY KEY,
    GenreName VARCHAR(50)
);

-- Creating a publisher table
CREATE TABLE IF NOT EXISTS Publisher (
    PublisherID INT AUTO_INCREMENT PRIMARY KEY,
    PublisherName VARCHAR(100),
    Address VARCHAR(255),
    Country VARCHAR(255)
);

-- Creating a table of books
CREATE TABLE IF NOT EXISTS Book (
    BookID INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(100),
    ISBN VARCHAR(20),
    PublicationYear INT,
    AuthorID INT,
    PublisherID INT,
    FOREIGN KEY (AuthorID) REFERENCES Author(AuthorID),
    FOREIGN KEY (PublisherID) REFERENCES Publisher(PublisherID)
);

-- Creating a table mapping genres and books
CREATE TABLE IF NOT EXISTS BookGenre (
    BookID INT,
    GenreID INT,
    PRIMARY KEY (BookID, GenreID),
    FOREIGN KEY (BookID) REFERENCES Book(BookID),
    FOREIGN KEY (GenreID) REFERENCES Genre(GenreID)
);