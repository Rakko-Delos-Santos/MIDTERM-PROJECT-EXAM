CREATE TABLE Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Subjects (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Code NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(500)
);

CREATE TABLE Sections (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SubjectId INT NOT NULL,
    FOREIGN KEY (SubjectId) REFERENCES Subjects(Id)
);

CREATE TABLE StudentSections (
    StudentId INT NOT NULL,
    SectionId INT NOT NULL,
    PRIMARY KEY (StudentId, SectionId),
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (SectionId) REFERENCES Sections(Id)
);