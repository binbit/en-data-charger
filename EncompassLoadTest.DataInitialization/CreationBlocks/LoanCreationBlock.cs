﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocVelocity.Integration.Encompass.API;
using Elli.Api.Loans.Model;
using EncompassLoadTest.DataInitialization.Creators;
using Monad;

namespace EncompassLoadTest.DataInitialization.CreationBlocks
{
    public class LoanCreationBlock : ICreationBlock
    {
        private readonly IEncompassClient _client;
        private readonly LoadConfiguration _loadConfiguration;
        private readonly DocumentCreationBlock _documentCreationBlock;

        public LoanCreationBlock(IEncompassClient client, LoadConfiguration loadConfiguration)
        {
            _client = client;
            _loadConfiguration = loadConfiguration;
            _documentCreationBlock = new DocumentCreationBlock(client, _loadConfiguration);
        }

        public async Task<IResult> CreateAsync(IResult result, string parentId)
        {
            var creator = new LoanCreator(_client, GetData(), parentId);
            var docTasks = new List<Task<IResult>>();
            for (var i = 0; i < _loadConfiguration.LoanNumberPerInstance; i++)
            {
                var res = creator.Create(parentId);
                res.Match(Success: r =>
                    {
                        result.AddResult(r);
                        docTasks.Add(Task.Run(() => _documentCreationBlock.CreateAsync(r, r.EntityId)));
                    },
                    Fail: f => result.AddError(new ResultError(parentId, f))).Invoke();

                await Task.Delay(_loadConfiguration.LoanCreationDelay);
            }

            await Task.WhenAll(docTasks);

            return result;
        }

        private LoanContract GetData()
        {
            return new LoanContract
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
            };
        }
    }
}