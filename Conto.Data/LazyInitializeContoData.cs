using System;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace Conto.Data
{
    public class LazyInitializeContoData
    {
        private static readonly Lazy<LazyInitializeContoData> Instance =
            new Lazy<LazyInitializeContoData>(() => new LazyInitializeContoData());

        private SqlCeConnection Connection
        {
            get
            {
                return new SqlCeConnection(ConfigurationManager.ConnectionStrings["ContoDatabase"].ConnectionString);
            }
        }

        private LazyInitializeContoData()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var settings = conn.Query<Settings>("SELECT * FROM Settings");
                if (!settings.Any())
                {
                    conn.Execute(
                        "INSERT INTO Settings (InvoiceOwnerName, InvoiceOwnerAddress, InvoiceOwnerCity, InvoiceOwnerPostalCode, InvoiceOwnerFiscalCode, InvoiceOwnerVatCode, MaxInvoiceValue) VALUES (@InvoiceOwnerName, @InvoiceOwnerAddress, @InvoiceOwnerCity, @InvoiceOwnerPostalCode, @InvoiceOwnerFiscalCode, @InvoiceOwnerVatCode, @MaxInvoiceValue)",
                        new { InvoiceOwnerName = "O.S. Trading S.r.l Soc. Unipersonale", InvoiceOwnerAddress = "Via Mascagni snc", InvoiceOwnerCity = "Usmate Velate", InvoiceOwnerPostalCode = "20040", InvoiceOwnerFiscalCode = "05962770961", InvoiceOwnerVatCode = "05962770961", MaxInvoiceValue = 990 });
                }

                var demoValues = ConfigurationManager.AppSettings["DemoValues"];
                if (demoValues == "true")
                {
                    #region COMMON

                    var common = conn.Query<Common>("SELECT * FROM Common");
                    if (!common.Any())
                    {
                        conn.Execute(
                            "INSERT INTO Common (CurrentAvailableInvoiceNumber, WorkYear) VALUES (@CurrentAvailableInvoiceNumber, @WorkYear)",
                            new {CurrentAvailableInvoiceNumber = 1, WorkYear = 2014});
                    }

                    #endregion

                    #region CLIENTS

                    var clients = conn.Query<Client>("SELECT * FROM Clients");
                    if (!clients.Any())
                    {
                        conn.Execute(
                            "INSERT INTO Clients (Name, Address, City, PostalCode, FiscalCode, VatCode) VALUES (@Name, @Address, @City, @PostalCode, @FiscalCode, @VatCode)",
                            new[]
                            {
                                new
                                {
                                    Name = "Rottami metalli Italia S.p.A",
                                    Address = "via G. Galilei 19",
                                    City = "Castelnuovo del Garda (VR)",
                                    PostalCode = "37014",
                                    FiscalCode = "03714080235",
                                    VatCode = "03714080235"
                                },
                                new
                                {
                                    Name = "R & R srl",
                                    Address = "via Roma",
                                    City = "Treviolo (BG)",
                                    PostalCode = "24048",
                                    FiscalCode = "03501480168",
                                    VatCode = "03501480168"
                                },
                                new
                                {
                                    Name = "Pianigiani Rottami S.r.l.",
                                    Address = "Strada di Ribucciano, 1-3-5-7-9",
                                    City = "Siena",
                                    PostalCode = "53100",
                                    FiscalCode = "12345678901",
                                    VatCode = "12345678901"
                                },
                                new
                                {
                                    Name = "DAINESE ROTTAMI SRL",
                                    Address = "Via Chiusa, 78/80",
                                    City = "Sant'Angelo di Piove di Sacco (Pd)",
                                    PostalCode = "35020",
                                    FiscalCode = "04284080282",
                                    VatCode = "04284080282"
                                },
                                new
                                {
                                    Name = "Sider Rottami Adriatica spa",
                                    Address = "Via delle Acacie",
                                    City = "Pesaro (PU)",
                                    PostalCode = "61122",
                                    FiscalCode = "12345678901",
                                    VatCode = "12345678901"
                                },
                                new
                                {
                                    Name = "SI.DE.COM. Srl - Soc. Unipersonale",
                                    Address = "Piazza Giuseppe Di Vittorio n. 114/2",
                                    City = "Frassinelle Polesine (RO)",
                                    PostalCode = "45030",
                                    FiscalCode = "01082400290",
                                    VatCode = "01082400290"
                                },
                                new
                                {
                                    Name = "FRATELLI LUCATI srl",
                                    Address = "Viale Del Del Lavoro 82",
                                    City = "Ponte San Nicolo' (PD)",
                                    PostalCode = "35020",
                                    FiscalCode = "00825290281",
                                    VatCode = "00825290281"
                                },
                                new
                                {
                                    Name = "SIMONE ROTTAMI srl",
                                    Address = "ZONA INDUSTRIALE SNC",
                                    City = "Matino (LE)",
                                    PostalCode = "73044",
                                    FiscalCode = "04407170754",
                                    VatCode = "04407170754"
                                },
                                new
                                {
                                    Name = "MORI SAURO ROTTAMI Srl Unipersonale",
                                    Address = "Strada di Ribucciano, 6",
                                    City = "Siena",
                                    PostalCode = "53100",
                                    FiscalCode = "01350050520",
                                    VatCode = "01350050520"
                                },
                                new
                                {
                                    Name = "SANTINI ROTTAMI srl",
                                    Address = "Via Partigiani D'Italia 28/A/B",
                                    City = "Empoli (FI)",
                                    PostalCode = "50053",
                                    FiscalCode = "01564580486",
                                    VatCode = "01564580486"
                                },
                                new
                                {
                                    Name = "SIR - SOCIETA' ITALIANA ROTTAMI srl",
                                    Address = "VIA VALDARNO 51",
                                    City = "CASSANO MAGNAGO (VA)",
                                    PostalCode = "21012",
                                    FiscalCode = "01417280128",
                                    VatCode = "01417280128"
                                }
                            });
                    }

                    #endregion

                    #region MATERIALS

                    var materials = conn.Query<Settings>("SELECT * FROM Materials");
                    if (!materials.Any())
                    {
                        conn.Execute(
                            "INSERT INTO Materials (Description, Price, MeasureId) VALUES (@Description, @Price, @MeasureId)",
                            new[]
                            {
                                new
                                {
                                    Description = "Rottame RAME 1^ CAT. S.",
                                    Price = 6270.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME 2^ CAT. N. (Tubo e Lastra vecchi)",
                                    Price = 5750.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME 2^ CAT. S. (Tubo e Lastra nuovi)",
                                    Price = 5950.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME 2^3^ CAT. MISTO",
                                    Price = 5630.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME 4^ CAT. (Caldaiette)",
                                    Price = 5480.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME CAVI (resa 42%)",
                                    Price = 1990.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME GRANULATO (resa 98%)",
                                    Price = 5810.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RAME STAGNATO (Treccia)",
                                    Price = 5740.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame MOTORI ELETTRICI MISTI",
                                    Price = 5400.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame MOTORI ELETTRICI LAVATRICE",
                                    Price = 5423.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame ALLUMINIO CERCHI",
                                    Price = 1658.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame ALLUMINIO PROFILO P.F.",
                                    Price = 1618.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame ALLUMINIO PROFILO RACCOLTA tolleranza 2%",
                                    Price = 1518.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame LASTRA ALLUMINIO MISTA tolleranza 2%",
                                    Price = 1346.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame LASTRA ALLUMINIO Nuova Verniciata",
                                    Price = 1346.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame LASTRA BIANCA 3000/5000",
                                    Price = 1620.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame LASTRA e/o CORDE DI ALLUMINIO DOLCE 99,7 BIANCO pulito",
                                    Price = 1696.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame LASTRA OFFSET (senza carta)",
                                    Price = 1160.0M,
                                    MeasureId = 3
                                },
                                new
                                {
                                    Description = "Rottame RADIATORI Alluminio/Rame puliti",
                                    Price = 1262.0M,
                                    MeasureId = 3
                                }
                            });
                    }

                    #endregion

                    #region SELF INVOICES

                    var selfInvoices = conn.Query<Common>("SELECT * FROM SelfInvoices");
                    if (!selfInvoices.Any())
                    {
                        var selfInvocesDate = DateTime.Now;
                        var invoiceGroupId = Guid.NewGuid();
                        // 12.6667
                        // 12540
                        conn.Execute(
                            "INSERT INTO SelfInvoices (MaterialId, Quantity, VatExcept, InvoiceYear, MeasureId, InCashFlow, InvoiceDate, InvoiceCost, InvoiceGroupId) VALUES (@MaterialId, @Quantity, @VatExcept, @InvoiceYear, @MeasureId, @InCashFlow, @InvoiceDate, @InvoiceCost, @InvoiceGroupId)",
                            new[]
                            {
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.158M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 990.0M,
                                    InvoiceGroupId = invoiceGroupId
                                },
                                new
                                {
                                    MaterialId = 1,
                                    Quantity = 0.104M,
                                    VatExcept = true,
                                    InvoiceYear = 2014,
                                    MeasureId = 3,
                                    InCashFlow = false,
                                    InvoiceDate = selfInvocesDate,
                                    InvoiceCost = 660.0M,
                                    InvoiceGroupId = invoiceGroupId
                                }
                            });

                    }

                    #endregion

                    #region INVOICES

                    var invoices = conn.Query<Invoice>("SELECT * FROM Invoices");
                    if (!invoices.Any())
                    {
                        conn.Execute(
                            "INSERT INTO Invoices (ClientId, InvoiceDate, InvoiceYear, MeasureId) VALUES (@ClientId, @InvoiceDate, @InvoiceYear, @MeasureId)",
                            new[]
                            {
                                new{ ClientId = 1, InvoiceDate = DateTime.Now, InvoiceYear = DateTime.Now.Year, MeasureId = 1 },
                                new{ ClientId = 2, InvoiceDate = DateTime.Now, InvoiceYear = DateTime.Now.Year, MeasureId = 1 },
                                new{ ClientId = 3, InvoiceDate = DateTime.Now, InvoiceYear = DateTime.Now.Year, MeasureId = 1 },
                                new{ ClientId = 4, InvoiceDate = DateTime.Now, InvoiceYear = DateTime.Now.Year, MeasureId = 1 }
                            });

                        conn.Execute(
                            "INSERT INTO InvoiceContent (Description, InvoiceId) VALUES (@Description, @InvoiceId)",
                            new[]
                            {
                                new { Description = "Form. PRZ844829/13 del 16,01,14", InvoiceId = 1 },
                                new { Description = "Form. PRZ844831/13 del 17,01,14", InvoiceId = 1 },
                                new { Description = "Form. PRZ844832/13 del 17,01,14", InvoiceId = 1 },
                                new { Description = "Form. PRZ844835/13 del 21,01,14", InvoiceId = 2 },
                                new { Description = "Form. PRZ844833/13 del 20,01,14", InvoiceId = 2 },
                                new { Description = "Form. PRZ844833/13 del 20,01,14", InvoiceId = 3 },
                                new { Description = "Form. PRZ844840/13 del 27,01,14", InvoiceId = 4 },
                                new { Description = "Form. PRZ844840/13 del 27,01,14", InvoiceId = 4 },
                            });

                        conn.Execute(
                            "INSERT INTO InvoiceMaterialRow (Description, MaterialQuantity, MaterialPrice, MaterialCost, InvoiceContentId) VALUES (@Description, @MaterialQuantity, @MaterialPrice, @MaterialCost, @InvoiceContentId)",
                            new[]
                            {
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 1},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 1},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 2},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 2},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 2},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 2},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 3},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 4},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 4},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 4},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 5},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 5},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 6},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 7},
                                new {Description = "Rottame di ferro carrozzeria", MaterialQuantity = 2.75M, MaterialPrice = 190.0M, MaterialCost = 522.5M, InvoiceContentId = 7},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 7},
                                new {Description = "Rottame di ferro carrozzeria B", MaterialQuantity = 24.79M, MaterialPrice = 180.0M, MaterialCost = 4462.2M, InvoiceContentId = 8}
                            });
                    }

                    #endregion
                }

            }
        }

        public static LazyInitializeContoData GetInstance
        {
            get { return Instance.Value; }
        }
    }
}
