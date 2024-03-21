using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Migrations
{
    public partial class AddTableGReceiptSubconAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentReceiptSubconPackingListModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    LocalSalesNoteNo = table.Column<string>(maxLength: 50, nullable: true),
                    LocalSalesNoteDate = table.Column<DateTimeOffset>(nullable: false),
                    LocalSalesContractId = table.Column<int>(nullable: false),
                    LocalSalesContractNo = table.Column<string>(maxLength: 50, nullable: true),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    TransactionTypeCode = table.Column<string>(maxLength: 25, nullable: true),
                    TransactionTypeName = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerNPWP = table.Column<string>(maxLength: 50, nullable: true),
                    PaymentTerm = table.Column<string>(maxLength: 25, nullable: true),
                    Omzet = table.Column<bool>(nullable: false),
                    Accounting = table.Column<bool>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false),
                    NettWeight = table.Column<double>(nullable: false),
                    NetNetWeight = table.Column<double>(nullable: false),
                    TotalCartons = table.Column<double>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    InvoiceNo = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTimeOffset>(nullable: false),
                    IsValidatedMD = table.Column<bool>(nullable: false),
                    ValidatedMDBy = table.Column<string>(maxLength: 100, nullable: true),
                    ValidatedMDDate = table.Column<DateTimeOffset>(nullable: true),
                    Kurs = table.Column<double>(nullable: false),
                    ValidatedMDRemark = table.Column<string>(maxLength: 1000, nullable: true),
                    IsValidatedShipping = table.Column<bool>(nullable: false),
                    ValidatedShippingBy = table.Column<string>(maxLength: 100, nullable: true),
                    ValidatedShippingDate = table.Column<DateTimeOffset>(nullable: true),
                    RejectReason = table.Column<string>(maxLength: 1000, nullable: true),
                    RejectTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentReceiptSubconPackingListModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentShippingLocalCoverLetterTSModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    NoteNo = table.Column<string>(maxLength: 50, nullable: true),
                    LocalCoverLetterNo = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    BuyerAdddress = table.Column<string>(maxLength: 1000, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    BCNo = table.Column<string>(maxLength: 50, nullable: true),
                    BCDate = table.Column<DateTimeOffset>(nullable: false),
                    Truck = table.Column<string>(maxLength: 250, nullable: true),
                    PlateNumber = table.Column<string>(maxLength: 250, nullable: true),
                    Driver = table.Column<string>(maxLength: 250, nullable: true),
                    ShippingStaffId = table.Column<int>(nullable: false),
                    ShippingStaffName = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentShippingLocalCoverLetterTSModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentShippingLocalSalesDOTSModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LocalSalesDONo = table.Column<string>(maxLength: 50, nullable: true),
                    LocalSalesNoteNo = table.Column<string>(maxLength: 50, nullable: true),
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    To = table.Column<string>(maxLength: 255, nullable: true),
                    StorageDivision = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(maxLength: 3000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentShippingLocalSalesDOTSModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentShippingLocalSalesNoteTSModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    SalesContractNo = table.Column<string>(maxLength: 50, nullable: true),
                    SalesContractId = table.Column<int>(nullable: false),
                    NoteNo = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 250, nullable: true),
                    BuyerNPWP = table.Column<string>(maxLength: 50, nullable: true),
                    KaberType = table.Column<string>(maxLength: 20, nullable: true),
                    PaymentType = table.Column<string>(maxLength: 20, nullable: true),
                    Tempo = table.Column<int>(nullable: false),
                    UseVat = table.Column<bool>(nullable: false),
                    VatId = table.Column<int>(nullable: false),
                    VatRate = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false),
                    IsCL = table.Column<bool>(nullable: false),
                    IsDO = table.Column<bool>(nullable: false),
                    IsDetail = table.Column<bool>(nullable: false),
                    IsApproveShipping = table.Column<bool>(nullable: false),
                    IsApproveFinance = table.Column<bool>(nullable: false),
                    ApproveShippingBy = table.Column<string>(nullable: true),
                    ApproveFinanceBy = table.Column<string>(nullable: true),
                    ApproveShippingDate = table.Column<DateTimeOffset>(nullable: false),
                    ApproveFinanceDate = table.Column<DateTimeOffset>(nullable: false),
                    IsRejectedFinance = table.Column<bool>(nullable: false),
                    IsRejectedShipping = table.Column<bool>(nullable: false),
                    RejectedReason = table.Column<string>(nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentShippingLocalSalesNoteTSModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GarmentReceiptSubconPackingListItemModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    PackingListId = table.Column<int>(nullable: false),
                    RONo = table.Column<string>(maxLength: 50, nullable: true),
                    PackingOutNo = table.Column<string>(maxLength: 50, nullable: true),
                    TotalQuantityPackingOut = table.Column<double>(nullable: false),
                    SCNo = table.Column<string>(maxLength: 50, nullable: true),
                    BuyerBrandId = table.Column<int>(nullable: false),
                    BuyerBrandName = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityId = table.Column<int>(nullable: false),
                    ComodityCode = table.Column<string>(maxLength: 50, nullable: true),
                    ComodityName = table.Column<string>(maxLength: 255, nullable: true),
                    ComodityDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    MarketingName = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 50, nullable: true),
                    PriceRO = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    PriceFOB = table.Column<double>(nullable: false),
                    PriceCMT = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Valas = table.Column<string>(maxLength: 50, nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    UnitCode = table.Column<string>(maxLength: 50, nullable: true),
                    Article = table.Column<string>(maxLength: 1000, nullable: true),
                    OrderNo = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DescriptionMd = table.Column<string>(maxLength: 1000, nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentReceiptSubconPackingListItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentReceiptSubconPackingListItemModel_GarmentReceiptSubconPackingListModel_PackingListId",
                        column: x => x.PackingListId,
                        principalTable: "GarmentReceiptSubconPackingListModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentShippingLocalSalesDOTSItemModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LocalSalesDOId = table.Column<int>(nullable: false),
                    LocalSalesNoteItemId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    PackQuantity = table.Column<double>(nullable: false),
                    PackUomId = table.Column<int>(nullable: false),
                    PackUomUnit = table.Column<string>(maxLength: 100, nullable: true),
                    GrossWeight = table.Column<double>(nullable: false),
                    NettWeight = table.Column<double>(nullable: false),
                    InvoiceNo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentShippingLocalSalesDOTSItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentShippingLocalSalesDOTSItemModel_GarmentShippingLocalSalesDOTSModel_LocalSalesDOId",
                        column: x => x.LocalSalesDOId,
                        principalTable: "GarmentShippingLocalSalesDOTSModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentShippingLocalSalesNoteTSItemModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LocalSalesNoteId = table.Column<int>(nullable: false),
                    PackingListId = table.Column<int>(nullable: false),
                    InvoiceNo = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    UomId = table.Column<int>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 250, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    PackageQuantity = table.Column<double>(nullable: false),
                    PackageUomId = table.Column<int>(nullable: false),
                    PackageUomUnit = table.Column<string>(maxLength: 250, nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentShippingLocalSalesNoteTSItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentShippingLocalSalesNoteTSItemModel_GarmentShippingLocalSalesNoteTSModel_LocalSalesNoteId",
                        column: x => x.LocalSalesNoteId,
                        principalTable: "GarmentShippingLocalSalesNoteTSModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentReceiptSubconPackingListDetailModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    PackingListItemId = table.Column<int>(nullable: false),
                    Carton1 = table.Column<double>(nullable: false),
                    Carton2 = table.Column<double>(nullable: false),
                    Style = table.Column<string>(maxLength: 100, nullable: true),
                    CartonQuantity = table.Column<double>(nullable: false),
                    QuantityPCS = table.Column<double>(nullable: false),
                    TotalQuantity = table.Column<double>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    Width = table.Column<double>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    GrossWeight = table.Column<double>(nullable: false),
                    NetWeight = table.Column<double>(nullable: false),
                    NetNetWeight = table.Column<double>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentReceiptSubconPackingListDetailModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentReceiptSubconPackingListDetailModel_GarmentReceiptSubconPackingListItemModel_PackingListItemId",
                        column: x => x.PackingListItemId,
                        principalTable: "GarmentReceiptSubconPackingListItemModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarmentReceiptSubconPackingListDetailSizeModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 128, nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 128, nullable: true),
                    PackingListDetailId = table.Column<int>(nullable: false),
                    PackingOutItemId = table.Column<Guid>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    Size = table.Column<string>(maxLength: 100, nullable: true),
                    SizeIdx = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Color = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentReceiptSubconPackingListDetailSizeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarmentReceiptSubconPackingListDetailSizeModel_GarmentReceiptSubconPackingListDetailModel_PackingListDetailId",
                        column: x => x.PackingListDetailId,
                        principalTable: "GarmentReceiptSubconPackingListDetailModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GarmentReceiptSubconPackingListDetailModel_PackingListItemId",
                table: "GarmentReceiptSubconPackingListDetailModel",
                column: "PackingListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentReceiptSubconPackingListDetailSizeModel_PackingListDetailId",
                table: "GarmentReceiptSubconPackingListDetailSizeModel",
                column: "PackingListDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentReceiptSubconPackingListItemModel_PackingListId",
                table: "GarmentReceiptSubconPackingListItemModel",
                column: "PackingListId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentShippingLocalSalesDOTSItemModel_LocalSalesDOId",
                table: "GarmentShippingLocalSalesDOTSItemModel",
                column: "LocalSalesDOId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentShippingLocalSalesNoteTSItemModel_LocalSalesNoteId",
                table: "GarmentShippingLocalSalesNoteTSItemModel",
                column: "LocalSalesNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentShippingLocalSalesNoteTSModel_NoteNo",
                table: "GarmentShippingLocalSalesNoteTSModel",
                column: "NoteNo",
                unique: true,
                filter: "[IsDeleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentReceiptSubconPackingListDetailSizeModel");

            migrationBuilder.DropTable(
                name: "GarmentShippingLocalCoverLetterTSModel");

            migrationBuilder.DropTable(
                name: "GarmentShippingLocalSalesDOTSItemModel");

            migrationBuilder.DropTable(
                name: "GarmentShippingLocalSalesNoteTSItemModel");

            migrationBuilder.DropTable(
                name: "GarmentReceiptSubconPackingListDetailModel");

            migrationBuilder.DropTable(
                name: "GarmentShippingLocalSalesDOTSModel");

            migrationBuilder.DropTable(
                name: "GarmentShippingLocalSalesNoteTSModel");

            migrationBuilder.DropTable(
                name: "GarmentReceiptSubconPackingListItemModel");

            migrationBuilder.DropTable(
                name: "GarmentReceiptSubconPackingListModel");
        }
    }
}
