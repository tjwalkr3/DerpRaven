-- Initialization script for the DerpRaven database

CREATE TABLE public."Images" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Alt" text NOT NULL,
    "Path" text NOT NULL,
    CONSTRAINT "PK_Images" PRIMARY KEY ("Id")
);

CREATE TABLE public."ProductTypes" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_ProductTypes" PRIMARY KEY ("Id")
);

CREATE TABLE public."Users" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Name" text NOT NULL,
    "OAuth" text NOT NULL,
    "Email" text NOT NULL,
    "Role" text NOT NULL,
    "Active" bool NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);
CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE public."CustomRequests" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Description" text NOT NULL,
    "Email" text NOT NULL,
    "Status" text NOT NULL,
    "ProductTypeId" int4 NOT NULL,
    "UserId" int4 NOT NULL,
    CONSTRAINT "PK_CustomRequests" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CustomRequests_ProductTypes_ProductTypeId" FOREIGN KEY ("ProductTypeId") REFERENCES public."ProductTypes"("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_CustomRequests_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_CustomRequests_ProductTypeId" ON public."CustomRequests" USING btree ("ProductTypeId");
CREATE INDEX "IX_CustomRequests_UserId" ON public."CustomRequests" USING btree ("UserId");

CREATE TABLE public."Portfolios" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Description" text NOT NULL,
    "Name" text NOT NULL,
    "ProductTypeId" int4 NOT NULL,
    CONSTRAINT "PK_Portfolios" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Portfolios_ProductTypes_ProductTypeId" FOREIGN KEY ("ProductTypeId") REFERENCES public."ProductTypes"("Id") ON DELETE RESTRICT
);
CREATE INDEX "IX_Portfolios_ProductTypeId" ON public."Portfolios" USING btree ("ProductTypeId");

CREATE TABLE public."Products" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Name" text NOT NULL,
    "Price" numeric NOT NULL,
    "Quantity" int4 NOT NULL,
    "Description" text NOT NULL,
    "ProductTypeId" int4 NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_ProductTypes_ProductTypeId" FOREIGN KEY ("ProductTypeId") REFERENCES public."ProductTypes"("Id") ON DELETE RESTRICT
);
CREATE INDEX "IX_Products_ProductTypeId" ON public."Products" USING btree ("ProductTypeId");

CREATE TABLE public."Orders" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Address" text NOT NULL,
    "Email" text NOT NULL,
    "OrderDate" timestamptz NOT NULL,
    "UserId" int4 NOT NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Orders_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_Orders_UserId" ON public."Orders" USING btree ("UserId");

CREATE TABLE public."PortfolioImage" (
    "ImageId" int4 NOT NULL,
    "PortfolioId" int4 NOT NULL,
    CONSTRAINT "PK_PortfolioImage" PRIMARY KEY ("ImageId", "PortfolioId"),
    CONSTRAINT "FK_PortfolioImage_Images_ImageId" FOREIGN KEY ("ImageId") REFERENCES public."Images"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PortfolioImage_Portfolios_PortfolioId" FOREIGN KEY ("PortfolioId") REFERENCES public."Portfolios"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_PortfolioImage_PortfolioId" ON public."PortfolioImage" USING btree ("PortfolioId");

CREATE TABLE public."ProductImage" (
    "ImageId" int4 NOT NULL,
    "ProductId" int4 NOT NULL,
    CONSTRAINT "PK_ProductImage" PRIMARY KEY ("ImageId", "ProductId"),
    CONSTRAINT "FK_ProductImage_Images_ImageId" FOREIGN KEY ("ImageId") REFERENCES public."Images"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ProductImage_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES public."Products"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_ProductImage_ProductId" ON public."ProductImage" USING btree ("ProductId");

CREATE TABLE public."OrderedProducts" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "Quantity" int4 NOT NULL,
    "Price" numeric NOT NULL,
    "Name" text NOT NULL,
    "OrderId" int4 NOT NULL,
    CONSTRAINT "PK_OrderedProducts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OrderedProducts_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES public."Orders"("Id") ON DELETE CASCADE
);
CREATE INDEX "IX_OrderedProducts_OrderId" ON public."OrderedProducts" USING btree ("OrderId");

INSERT INTO public."ProductTypes" ("Name") VALUES
    ('Plushie'),
    ('Art');

INSERT INTO public."Users" ("Name","OAuth","Email","Role","Active") VALUES
    ('Derp','dljapfpsadkfjpajdpfajd','Derpipose@gmail.com','Admin',true),
    ('Derp2','jhkjhljhkwejifuhiwine','Derpipose2@gmail.com','Customer',false);

INSERT INTO public."Products" ("Name","Price","Quantity","Description","ProductTypeId") VALUES
    ('Product1',10,1,'Plushie',1),
    ('Product2',25,1,'Art',2);

INSERT INTO public."Orders" ("Address","Email","OrderDate","UserId") VALUES
    ('Madeup1','Derpipose@gmail.com','2025-03-27 00:00:00.000',1),
    ('Madeup2','Derpipose@gmail.com','2025-01-15 00:00:00.000',2);

INSERT INTO public."CustomRequests" ("Description","Email","Status","ProductTypeId","UserId") VALUES
    ('this Plushie','Derpipose@gmail.com','pending',1,1),
    ('this Art','Derpipose2@gmail.com','pending',2,2),
    ('this Art','Derpipose@gmail.com','pending',2,1);

INSERT INTO public."Images" ("Alt","Path") VALUES
    ('thisimage1','imagepath1'),
    ('thisimage2','imagepath2');

INSERT INTO public."Portfolios" ("Description","Name","ProductTypeId") VALUES
    ('portfolioPlush','Portfolio1',1),
    ('portfolioArt','Portfolio2',2);

INSERT INTO public."PortfolioImage" ("ImageId","PortfolioId") VALUES
    (1,1),
    (2,2);

INSERT INTO public."ProductImage" ("ImageId","ProductId") VALUES
    (1,1),
    (2,2);

INSERT INTO public."OrderedProducts" ("Quantity","Price","Name","OrderId") VALUES
    (2,22.50,'example plushie',1),
    (2,33.75,'example art',2);

INSERT INTO public."__EFMigrationsHistory" ("MigrationId","ProductVersion") VALUES
    ('20250322010505_InitialCreate','9.0.3'),
    ('20250407161114_UniqueEmail','9.0.3'),
    ('20250414181005_AddPurchasedProductTable','9.0.3'),
    ('20250414181847_RemoveProductIdFromOrder','9.0.3');
