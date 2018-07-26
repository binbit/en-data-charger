using System;
using System.Collections.Generic;
using System.Configuration;
using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.EFolder.Model;
using Elli.Api.Loans.Model;
using EncompassLoadTest.Properties;

namespace EncompassLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationManager.GetSection("ElliApiConfig");
            var client = new EncompassClient(config);
            var loanId = CreateLoan(client);
            var docId = CreateDocument(client, loanId);
            var attachmentId = UploadAttachment(client, loanId, null);
            client.DocumentService.AttachAttachments(docId, loanId, new List<EFolderEntityRefContract>
            {
                new EFolderEntityRefContract
                {
                    EntityId = attachmentId
                }
            });
        }

        private static string UploadAttachment(IEncompassClient client, string loanId, string documentId)
        {
            return client.AttachmentService.UploadAttachment(loanId, "Test File", "file.pdf",
                Resources.Affiliated_Business_Arrangement_Disclosure);
        }

        private static string CreateDocument(IEncompassClient client, string loanId)
        { 
            return client.DocumentService.CreateDocument(loanId, new EFolderDocumentContract
            {
                Title = "Test Document",
                Description = "It test document 8-)",
                ApplicationId = "All",
                RequestedFrom = "User",
                EmnSignature = "string",
                DateRequested = DateTime.Now,
                DateExpected = DateTime.Now,
                DateReceived = DateTime.Now,
                DateReviewed = DateTime.Now,
                DateReadyForUw = DateTime.Now,
                DateReadyToShip = DateTime.Now,
                Comments = new List<EFolderDocumentContractComments>
                {
                    new EFolderDocumentContractComments
                    {
                        Comments = "Lalala"
                    }
                }
            });
        }

        private static string CreateLoan(IEncompassClient client)
        {
            return client.LoanService.CreateLoan(new LoanContract
            {
                Applications = new List<LoanContractApplications>
                {
                    new LoanContractApplications
                    {
                        Borrower = new LoanContractBorrower
                        {
                            AltId = "borrower_1",
                            BirthDate = new DateTime(1991, 08, 09),
                            EmailAddressText = "email@email.com",
                            FirstName = "Load",
                            LastName = "Test",
                            HomePhoneNumber = "111-222-3333"
                        },
                        Coborrower = new LoanContractBorrower
                        {
                            AltId = "coborrower_1",
                            BirthDate = new DateTime(1991, 08, 09),
                            EmailAddressText = "email@email.com",
                            FirstName = "Load",
                            LastName = "Test",
                            HomePhoneNumber = "111-222-3333"
                        },
                        PropertyUsageType = "SecondHome",
                        Residences = new List<LoanContractResidences>
                        {
                            new LoanContractResidences
                            {
                                ApplicantType = "Borrower",
                                AddressCity = "Pleasanton",
                                AddressPostalCode = "94588",
                                AddressState = "CA"
                            }
                        },
                        Employment = new List<LoanContractEmployment>
                        {
                            new LoanContractEmployment
                            {
                                Owner = "Borrower",
                                CurrentEmploymentIndicator = true,
                                PhoneNumber = "111-222-3333"
                            }
                        },
                        TotalIncomeAmount = 25000,
                        Income = new List<LoanContractIncome>
                        {
                            new LoanContractIncome
                            {
                                IncomeType = "Base",
                                Owner = "Borrower",
                                Amount = 25000,
                                CurrentIndicator = true
                            }
                        }
                    }
                },
                Property = new LoanContractProperty
                {
                    BuildingStatusType = "",
                    LoanPurposeType = "",
                    PropertyRightsType = "",
                    PropertyUsageType = "Investment",
                    TypeRecordingJurisdiction = "County"
                }
            });

        }
    }
}
