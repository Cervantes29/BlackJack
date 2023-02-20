using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessDAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gamelogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PlayerCards = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerCards = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerScore = table.Column<int>(type: "int", nullable: false),
                    DealerScore = table.Column<int>(type: "int", nullable: false),
                    BalanceChange = table.Column<float>(type: "real", nullable: false),
                    Decision = table.Column<bool>(type: "bit", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamelogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PossibleDealerHands",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleDealerHands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PossiblePlayerHands",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossiblePlayerHands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    AllTimeBalance = table.Column<int>(type: "int", nullable: false),
                    Decision = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    ScenarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Round = table.Column<int>(type: "int", nullable: false),
                    PlayerHand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerHand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossibleDealerHandsID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PossiblePlayerHandsID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.ScenarioID);
                    table.ForeignKey(
                        name: "FK_Scenarios_PossibleDealerHands_PossibleDealerHandsID",
                        column: x => x.PossibleDealerHandsID,
                        principalTable: "PossibleDealerHands",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Scenarios_PossiblePlayerHands_PossiblePlayerHandsID",
                        column: x => x.PossiblePlayerHandsID,
                        principalTable: "PossiblePlayerHands",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_PossibleDealerHandsID",
                table: "Scenarios",
                column: "PossibleDealerHandsID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_PossiblePlayerHandsID",
                table: "Scenarios",
                column: "PossiblePlayerHandsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gamelogs");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PossibleDealerHands");

            migrationBuilder.DropTable(
                name: "PossiblePlayerHands");
        }
    }
}
