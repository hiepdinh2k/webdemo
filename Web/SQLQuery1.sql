USE [SVKMT]
GO
/****** Object:  StoredProcedure [dbo].[SP_login]    Script Date: 27-12-2021 4:15:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[lich_su](
	[NoiDung] [nvarchar](50) NULL
) ON [PRIMARY]

GO
-- =============================================
-- Author:		login
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_login]
	@action nvarchar(50),
	@msv nvarchar(50)=null,
	@pw nvarchar(50)=null,
	@pwc nvarchar(50)=null,
	@NoiDung nvarchar(50)=null
AS
BEGIN
	if(@action='login')
	begin 
	select * from SV where msv = @msv and pw = @pw;
	end
	else if(@action='pwm')
	begin 
	UPDATE SV SET pw =@pw  WHERE pw=@pwc;
	end
	else if(@action='lichsu')
	begin 
	INSERT INTO lich_su (NoiDung)VALUES (@NoiDung);
	end
END
