USE [My-t-shirt]
GO
/****** Object:  StoredProcedure [dbo].[Role_Login]    Script Date: 06/14/2021 14:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[Role_Login]
@user varchar(50),
@Password varchar(50)
as
begin
select *from Compte where LoginCpt=@user and PasswordCpt=@Password
End
GO
/****** Object:  Table [dbo].[Utilisateur]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Utilisateur](
	[idUtilisateur] [nvarchar](50) NOT NULL,
	[nom] [nvarchar](50) NULL,
	[prenom] [nvarchar](50) NULL,
	[email] [nvarchar](50) NULL,
	[motdepasse] [nvarchar](50) NULL,
	[isAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_Utilisateur] PRIMARY KEY CLUSTERED 
(
	[idUtilisateur] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Taille]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taille](
	[idTaille] [nvarchar](50) NOT NULL,
	[nom] [nvarchar](50) NULL,
 CONSTRAINT [PK_Taille] PRIMARY KEY CLUSTERED 
(
	[idTaille] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marque]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marque](
	[idMarque] [nvarchar](50) NOT NULL,
	[nom] [nvarchar](50) NULL,
 CONSTRAINT [PK_Marque] PRIMARY KEY CLUSTERED 
(
	[idMarque] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Commande]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Commande](
	[idCommande] [nvarchar](50) NOT NULL,
	[idUtilisateur] [nvarchar](50) NULL,
	[Adresse_Com] [nvarchar](100) NULL,
	[Ville_Com] [nvarchar](50) NULL,
	[CodePostal_Com] [varchar](20) NULL,
	[Pays_Com] [nvarchar](50) NULL,
	[Date_Com] [datetime] NULL,
	[isDelivered] [bit] NULL,
	[dateLivraison] [datetime] NULL,
 CONSTRAINT [PK_Commande] PRIMARY KEY CLUSTERED 
(
	[idCommande] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Review]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[idReview] [nvarchar](50) NOT NULL,
	[idUtilisateur] [nvarchar](50) NULL,
	[idProduit] [nvarchar](50) NULL,
	[Rating] [float] NULL,
	[Commentaire] [nvarchar](250) NULL,
	[dateReview] [date] NULL,
 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
(
	[idReview] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QteStock_Taille]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QteStock_Taille](
	[idTaille] [nvarchar](50) NOT NULL,
	[idProduit] [nvarchar](50) NOT NULL,
	[qteStock] [int] NULL,
 CONSTRAINT [PK_QteStock_Taille] PRIMARY KEY CLUSTERED 
(
	[idTaille] ASC,
	[idProduit] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ligneCommande]    Script Date: 06/14/2021 14:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ligneCommande](
	[idCommande] [nvarchar](50) NOT NULL,
	[idProduit] [nvarchar](50) NOT NULL,
	[qte] [int] NULL,
	[idTaille] [nvarchar](50) NOT NULL,
 CONSTRAINT [pk1] PRIMARY KEY CLUSTERED 
(
	[idCommande] ASC,
	[idProduit] ASC,
	[idTaille] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[getPrixSolde]    Script Date: 06/14/2021 14:49:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getPrixSolde](@idProduit nvarchar(50))
returns float as
	begin
	declare @nbr int
	declare @prix float
	select @nbr  = count(*) from solde where idProduit = @idProduit
	if @nbr = 0
		begin 
			select @prix = prix from produit where idProduit = @idProduit
		end
	else
		begin 
        select @prix = (prix-(prix* pourcentage)/100) 
					from produit p inner join solde s 
					on p.idProduit = s.idProduit
					where p.idProduit = @idProduit
		end
		
		return @prix
	end
GO
/****** Object:  Default [DF_Utilisateur_isAdmin]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_isAdmin]  DEFAULT ((0)) FOR [isAdmin]
GO
/****** Object:  Default [DF_Commande_isDelivered]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Commande] ADD  CONSTRAINT [DF_Commande_isDelivered]  DEFAULT ((0)) FOR [isDelivered]
GO
/****** Object:  Default [DF_Rating_Rating]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Review] ADD  CONSTRAINT [DF_Rating_Rating]  DEFAULT ((0)) FOR [Rating]
GO
/****** Object:  ForeignKey [FK_Produit_Marque]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Produit]  WITH CHECK ADD  CONSTRAINT [FK_Produit_Marque] FOREIGN KEY([idMarque])
REFERENCES [dbo].[Marque] ([idMarque])
GO
ALTER TABLE [dbo].[Produit] CHECK CONSTRAINT [FK_Produit_Marque]
GO
/****** Object:  ForeignKey [FK_Commande_Utilisateur]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Commande]  WITH CHECK ADD  CONSTRAINT [FK_Commande_Utilisateur] FOREIGN KEY([idUtilisateur])
REFERENCES [dbo].[Utilisateur] ([idUtilisateur])
GO
ALTER TABLE [dbo].[Commande] CHECK CONSTRAINT [FK_Commande_Utilisateur]
GO
/****** Object:  ForeignKey [FK_Rating_Produit]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Produit] FOREIGN KEY([idProduit])
REFERENCES [dbo].[Produit] ([idProduit])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Rating_Produit]
GO
/****** Object:  ForeignKey [FK_Rating_Utilisateur]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Utilisateur] FOREIGN KEY([idUtilisateur])
REFERENCES [dbo].[Utilisateur] ([idUtilisateur])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Rating_Utilisateur]
GO
/****** Object:  ForeignKey [FK_QteStock_Taille_Produit]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[QteStock_Taille]  WITH CHECK ADD  CONSTRAINT [FK_QteStock_Taille_Produit] FOREIGN KEY([idProduit])
REFERENCES [dbo].[Produit] ([idProduit])
GO
ALTER TABLE [dbo].[QteStock_Taille] CHECK CONSTRAINT [FK_QteStock_Taille_Produit]
GO
/****** Object:  ForeignKey [FK_QteStock_Taille_Taille]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[QteStock_Taille]  WITH CHECK ADD  CONSTRAINT [FK_QteStock_Taille_Taille] FOREIGN KEY([idTaille])
REFERENCES [dbo].[Taille] ([idTaille])
GO
ALTER TABLE [dbo].[QteStock_Taille] CHECK CONSTRAINT [FK_QteStock_Taille_Taille]
GO
/****** Object:  ForeignKey [FK_Solde_Produit]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[Solde]  WITH CHECK ADD  CONSTRAINT [FK_Solde_Produit] FOREIGN KEY([idProduit])
REFERENCES [dbo].[Produit] ([idProduit])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Solde] CHECK CONSTRAINT [FK_Solde_Produit]
GO
/****** Object:  ForeignKey [FK_ligneCommande_Commande]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[ligneCommande]  WITH CHECK ADD  CONSTRAINT [FK_ligneCommande_Commande] FOREIGN KEY([idCommande])
REFERENCES [dbo].[Commande] ([idCommande])
GO
ALTER TABLE [dbo].[ligneCommande] CHECK CONSTRAINT [FK_ligneCommande_Commande]
GO
/****** Object:  ForeignKey [FK_ligneCommande_Produit]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[ligneCommande]  WITH CHECK ADD  CONSTRAINT [FK_ligneCommande_Produit] FOREIGN KEY([idProduit])
REFERENCES [dbo].[Produit] ([idProduit])
GO
ALTER TABLE [dbo].[ligneCommande] CHECK CONSTRAINT [FK_ligneCommande_Produit]
GO
/****** Object:  ForeignKey [FK_ligneCommande_Taille]    Script Date: 06/14/2021 14:49:30 ******/
ALTER TABLE [dbo].[ligneCommande]  WITH CHECK ADD  CONSTRAINT [FK_ligneCommande_Taille] FOREIGN KEY([idTaille])
REFERENCES [dbo].[Taille] ([idTaille])
GO
ALTER TABLE [dbo].[ligneCommande] CHECK CONSTRAINT [FK_ligneCommande_Taille]
GO
