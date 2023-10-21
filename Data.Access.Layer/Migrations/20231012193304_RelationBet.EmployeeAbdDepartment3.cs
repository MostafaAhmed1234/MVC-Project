using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Access.Layer.Migrations
{
    public partial class RelationBetEmployeeAbdDepartment3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_Dept_ID",
                table: "Employees");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_Dept_ID",
                table: "Employees",
                column: "Dept_ID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
