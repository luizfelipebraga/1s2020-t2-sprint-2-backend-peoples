-- Define o banco de dados que será utilizado
USE Peoples;

-- Insere dois funcionários
INSERT INTO TipoUsuarios(NomeTipoUsuario)
VALUES ('Comum'),('Administrador');

INSERT INTO Usuarios(Email,Senha,IdTipoUsuario)
VALUES ('catarina@gmail.com','123',1),('tadeu@gmail.com','123',2);

INSERT INTO Funcionarios (Nome, Sobrenome,IdUsuario) 
VALUES	('Catarina', 'Strada',1) ,('Tadeu', 'Vitelli',2);

-- Atualiza o valor da coluna DataNascimento
UPDATE Funcionarios SET DataNascimento = '1993-03-17';

select * from Funcionarios

select * from Usuarios