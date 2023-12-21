using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike.Data.Migrations
{
    /// <inheritdoc />
    public partial class Trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[Time]
            ON  [dbo].[Routes]
            AFTER INSERT, UPDATE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;

                DECLARE @RouteId int, @Address1 int, @Address2 int, @BikeType int,  
                 @Time int, @newtime int; 

                SELECT TOP 1 @RouteId=[RouteId], @Address1=[AddressId1], @Address2=[AddressId2],
                    @BikeType=[BikeTypeId], @Time=[Time] FROM INSERTED;

                SET @BikeType = (SELECT [Time] FROM [dbo].[BikeType] WHERE [BikeTypeId]=@BikeType)
               
                SET @Address1 = (SELECT [MainAddressId] FROM [dbo].[Address] WHERE [AddressId]=@Address1)
                SET @Address1 = (SELECT [HeightId] FROM [dbo].[MainAddress] WHERE [MainAddressId]=@Address1)
                SET @Address1 = (SELECT [Time] FROM [dbo].[Height] WHERE [HeightId]=@Address1)

                SET @Address2 = (SELECT [MainAddressId] FROM [dbo].[Address] WHERE [AddressId]=@Address2)
                SET @Address2 = (SELECT [HeightId] FROM [dbo].[MainAddress] WHERE [MainAddressId]=@Address2)
                SET @Address2 = (SELECT [Time] FROM [dbo].[Height] WHERE [HeightId]=@Address2)

                SET @newtime = @Address1 + @Address2 + @BikeType + @Time
                UPDATE [dbo].[Routes] SET [TimeResult] = @newtime WHERE [RouteId]=@RouteId
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Time]");
        }
    }
}
