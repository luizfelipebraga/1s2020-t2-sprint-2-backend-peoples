-- Cria o banco de dados
CREATE DATABASE Peoples;

-- Define qual banco de dados será utilizado
USE Peoples;

-- Cria a tabela Funcionarios

CREATE TABLE TipoUsuarios (
	IdTipoUsuario INT PRIMARY KEY IDENTITY,
	NomeTipoUsuario VARCHAR(200) NOT NULL
);

CREATE TABLE Usuarios(
	IdUsuario INT PRIMARY KEY IDENTITY,
	Email VARCHAR (250) not null unique,
	Senha VARCHAR (250) not null ,
	IdTipoUsuario INT FOREIGN KEY REFERENCES TipoUsuarios(IdTipoUsuario)
);

CREATE TABLE Funcionarios 
(
	IdFuncionario	INT IDENTITY PRIMARY KEY
	,Nome			VARCHAR(200) NOT NULL
	,Sobrenome		VARCHAR(255)
	,IdUsuario INT FOREIGN KEY REFERENCES Usuarios (IdUsuario)
);

GO


-- Adiciona a coluna DataNascimento na tabela Funcionarios
ALTER TABLE Funcionarios
ADD DataNascimento DATE