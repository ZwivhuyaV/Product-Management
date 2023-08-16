--CREATE DATABASE ProductManagement;


--Insert into [dbo].[Role] (RoleName, IsActive, CreatedDate, UpdatedDate)
--select 'Admin', 1, GETDATE(), null;

--Insert into [dbo].[Role] (RoleName, IsActive, CreatedDate, UpdatedDate)
--select 'Normal', 1, GETDATE(), null;

select * from [dbo].[Role]

--Insert into Employee(Firstname, LastName, Email, [Password], RoleId, IsActive, CreatedDate)
--select 'The', 'Administrator', 'morganzwivhuya@gmail.com','P@ssword93!',1,1,GETDATE()

select * from [dbo].[Employee]