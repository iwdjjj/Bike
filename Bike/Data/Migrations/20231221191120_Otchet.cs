using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike.Data.Migrations
{
    /// <inheritdoc />
    public partial class Otchet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE Otchet
	        --@id int
            AS
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;

                -- Insert statements for procedure here
	            SELECT N.[BikeTypeId] id,[Name] nm, COUNT(P.RouteId) kol FROM [dbo].[BikeType] N
		            JOIN [dbo].[Routes] P ON N.BikeTypeId=P.BikeTypeId
			            --WHERE RouteId=@id
	            --GROUP BY [Name]
                GROUP BY N.[BikeTypeId],[Name]
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE Otchet");
        }
    }
}
