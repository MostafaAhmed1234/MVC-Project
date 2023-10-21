using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Access.Layer.Migrations
{
    public partial class RelationBetEmployeeAbdDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dept_ID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Dept_ID",
                table: "Employees",
                column: "Dept_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_Dept_ID",
                table: "Employees",
                column: "Dept_ID",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_Dept_ID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Dept_ID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Dept_ID",
                table: "Employees");
        }
    }
}
