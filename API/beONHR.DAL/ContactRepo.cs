using beONHR.Entities.DTO;
using beONHR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.Context;
using Azure;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO.Enum;
using MimeKit;
using static beONHR.Entities.Permissions;
using System.Formats.Asn1;
using beONHR.Entities.User;
using System.Reflection;
using System.IO;
using beONHR.Entities.DTO.beONHR.Entities.DTO;
using beONHR.Entities.beONHR.Entities;

namespace beONHR.DAL
{
    public interface IContactRepo
    {
        Task<ClientResponse> SaveContact(ContactDTO input);

        Task<ClientResponse> UpdateContact(ContactDTO input);
        Task<ClientResponse> GetContact();
        Task<ClientResponse> GetContactById(Guid id);
        Task<ClientResponse> GetContactByEmployeeId(Guid employeeId);

    }
    public class ContactRepo : IContactRepo
    {
        private readonly MainContext _context;
        public ContactRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveContact(ContactDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {

                    var contact = await _context.Contacts.Where(x => x.WorkZipCode == input.WorkZipCode && x.IsDeleted != true).ToListAsync(); 


                    if (contact.Count() == 0)
                    {
                        Contact model = new Contact()
                        {
                            Id = Guid.NewGuid(),
                            WorkZipCode = input.WorkZipCode,
                            WorkCity = input.WorkCity,
                            WorkStateId = input.WorkStateId,
                            WorkCountryId = input.WorkCountryId,
                            EmployeeId= input.EmployeeId,



                            ContactAdressDetails = input.ContactAdressDetails.Select(c => new ContactAdress
                            {
                                Id = Guid.NewGuid(),
                                Street = c.Street,
                                ContactStateId = c.ContactStateId,
                                ContactCountryId = c.ContactCountryId,
                                ContactCity = c.ContactCity,

                                Number = c.Number,

                                ContactZipCode = c.ContactZipCode,
                                ContactPhone1 = c.ContactPhone1,
                                ContactPhone2 = c.ContactPhone2,
                                ContactEmailbeON = c.ContactEmailbeON,
                                ContactEmailPrivate = c.ContactEmailPrivate,
                                ContactEntitlement = c.ContactEntitlement,
                            }).ToList(),


                            Bankdetails = input.BankDetails.Select(c => new BankDetails
                            {
                                Id = Guid.NewGuid(),
                                BankAccountNumber = c.BankAccountNumber,
                                BankIFSCCode = c.BankIFSCCode,
                                BankName = c.BankName,
                                BankAccountHolder = c.BankAccountHolder,
                            }).ToList()
                        };


                        await _context.Contacts.AddAsync(model);


                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Contacts not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Contacts inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Contacts already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }

                    return response;
                }
                else
                {
                    var existingcontact = await _context.Contacts
                                            .Include(e => e.ContactAdressDetails)
                                            .Include(b => b.Bankdetails)
                                            .FirstOrDefaultAsync(x =>x.Id==input.Id  && x.IsDeleted == false);

                    if (existingcontact != null)
                    {

                        
                        existingcontact.WorkZipCode = input.WorkZipCode;
                        existingcontact.WorkCity = input.WorkCity;
                        existingcontact.WorkStateId = input.WorkStateId;
                        existingcontact.WorkCountryId = input.WorkCountryId;
                        existingcontact.EmployeeId = input.EmployeeId;

                        if (existingcontact.Bankdetails != null && existingcontact.Bankdetails.Count() != 0)
                        {
                            _context.Bankdetails.RemoveRange(existingcontact.Bankdetails);
                        }


                        existingcontact.Bankdetails = input.BankDetails.Select(c => new BankDetails
                        {
                            Id = c.Id,
                            BankAccountNumber = c.BankAccountNumber,
                            BankIFSCCode = c.BankIFSCCode,
                            BankName = c.BankName,
                            BankAccountHolder = c.BankAccountHolder
                        }).ToList();


                        if (existingcontact.ContactAdressDetails != null && existingcontact.ContactAdressDetails.Count() != 0)
                        {

                            _context.ContactAdresses.RemoveRange(existingcontact.ContactAdressDetails);
                        }

                        

                        existingcontact.ContactAdressDetails = input.ContactAdressDetails.Select(c => new ContactAdress
                        {
                            Id = c.Id,
                            Street = c.Street,
                            Number = c.Number,
                            ContactStateId = c.ContactStateId,
                            ContactCountryId = c.ContactCountryId,
                            ContactCity = c.ContactCity,
                            ContactZipCode = c.ContactZipCode,
                            ContactPhone1 = c.ContactPhone1,
                            ContactPhone2 = c.ContactPhone2,
                            ContactEmailbeON = c.ContactEmailbeON,
                            ContactEmailPrivate = c.ContactEmailPrivate,
                            ContactEntitlement = c.ContactEntitlement
                        }).ToList();

                        _context.Contacts.Update(existingcontact);


                        await _context.SaveChangesAsync();

                        response.Message = "contact updated successfully";
                        response.HttpResponse = existingcontact.Id;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "contact not found";
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.IsSuccess = false;
                    }
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while saving the existingcontact";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                throw ex;
            }
        }
        public async Task<ClientResponse> UpdateContact(ContactDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var existingContact = await _context.Contacts
                                          .Include(e => e.ContactAdressDetails)
                                          .Include(b => b.Bankdetails)
                                          .FirstOrDefaultAsync(x => x.Id == input.Id && !x.IsDeleted);

                if (existingContact != null)
                {
                    // Update contact details
                    existingContact.WorkZipCode = input.WorkZipCode;
                    existingContact.WorkCity = input.WorkCity;
                    existingContact.WorkStateId = input.WorkStateId;
                    existingContact.WorkCountryId = input.WorkCountryId;
                    existingContact.EmployeeId = input.EmployeeId;

                    // Update bank details
                    if (existingContact.Bankdetails != null && existingContact.Bankdetails.Any())
                    {
                        _context.Bankdetails.RemoveRange(existingContact.Bankdetails);
                    }
                    existingContact.Bankdetails = input.BankDetails.Select(c => new BankDetails
                    {
                        Id = c.Id,
                        BankAccountNumber = c.BankAccountNumber,
                        BankIFSCCode = c.BankIFSCCode,
                        BankName = c.BankName,
                        BankAccountHolder = c.BankAccountHolder
                    }).ToList();

                    // Update contact address details
                    if (existingContact.ContactAdressDetails != null && existingContact.ContactAdressDetails.Any())
                    {
                        _context.ContactAdresses.RemoveRange(existingContact.ContactAdressDetails);
                    }
                    existingContact.ContactAdressDetails = input.ContactAdressDetails.Select(c => new ContactAdress
                    {
                        Id = c.Id,
                        Street = c.Street,
                        Number = c.Number,
                        ContactStateId = c.ContactStateId,
                        ContactCountryId = c.ContactCountryId,
                        ContactCity = c.ContactCity,
                        ContactZipCode = c.ContactZipCode,
                        ContactPhone1 = c.ContactPhone1,
                        ContactPhone2 = c.ContactPhone2,
                        ContactEmailbeON = c.ContactEmailbeON,
                        ContactEmailPrivate = c.ContactEmailPrivate,
                        ContactEntitlement = c.ContactEntitlement
                    }).ToList();

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    response.Message = "Contact updated successfully";
                    response.HttpResponse = existingContact.Id;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    // Log an error if the contact was not found
                    response.Message = "Contact not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    Console.WriteLine($"Contact with ID {input.Id} not found.");
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions that occur during the update process
                response.Message = "An error occurred while updating the contact";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                Console.WriteLine($"Error updating contact: {ex.Message}");
                throw;
            }
        }


        public async Task<ClientResponse> GetContactByEmployeeId(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var contact = await _context.Contacts.Where(x => x.EmployeeId == id && x.IsDeleted != true).Include(x => x.ContactAdressDetails).Select(x => new ContactDTO
                {
                    Id =x.Id,
                    WorkZipCode = x.WorkZipCode,
                    WorkCity = x.WorkCity,
                    WorkStateId = x.WorkStateId,
                    WorkCountryId = x.WorkCountryId,
                    EmployeeId =x.EmployeeId,



                    BankDetails = x.Bankdetails.Select(c => new BankDetailsDTO
                    {
                        Id = c.Id,
                        BankAccountNumber = c.BankAccountNumber,
                        BankIFSCCode = c.BankIFSCCode,
                        BankName = c.BankName,
                        BankAccountHolder = c.BankAccountHolder
                    }).ToList(),
                




                ContactAdressDetails = x.ContactAdressDetails.Select(c => new ContactAdressDTO
                    {
                        Id = c.Id,
                        Street = c.Street,
                        Number = c.Number,
                        ContactStateId = c.ContactStateId,
                        ContactCountryId = c.ContactCountryId,
                        ContactCity = c.ContactCity,
                        ContactZipCode = c.ContactZipCode,
                        ContactPhone1 = c.ContactPhone1,
                        ContactPhone2 = c.ContactPhone2,
                        ContactEmailbeON = c.ContactEmailbeON,
                        ContactEmailPrivate = c.ContactEmailPrivate,
                        ContactEntitlement = c.ContactEntitlement
                    }).ToList(),
                }).FirstOrDefaultAsync();

                if (contact != null)
                {

                    response.Message = "Contacts Get Sucesfully";
                    response.HttpResponse = contact;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Contacts not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ClientResponse> GetContact()
        {
            ClientResponse response = new();
            try
            {
                var contact = await _context.Contacts
                 .Where(x => x.IsDeleted != true).Include(x => x.ContactAdressDetails)
                 .Select(x => new ContactDTO
                 {
                     Id=x.Id,
                     WorkZipCode = x.WorkZipCode,
                     WorkCity = x.WorkCity,
                     WorkStateId = x.WorkStateId,
                     WorkCountryId = x.WorkCountryId,
                     EmployeeId = x.EmployeeId,


                     ContactAdressDetails = x.ContactAdressDetails.Select(c => new ContactAdressDTO
                     {
                         Id = c.Id,
                         Number=c.Number,
                         Street = c.Street,
                         ContactStateId = c.ContactStateId,
                         ContactCountryId = c.ContactCountryId,
                         ContactZipCode = c.ContactZipCode,
                         ContactCity = c.ContactCity,
                         ContactPhone1 = c.ContactPhone1,
                         ContactPhone2 = c.ContactPhone2,
                         ContactEmailPrivate = c.ContactEmailPrivate,
                         ContactEmailbeON = c.ContactEmailbeON,
                         ContactEntitlement = c.ContactEntitlement,

                     }).ToList(),

                     BankDetails = x.Bankdetails.Select(c => new BankDetailsDTO
                     {
                         Id = c.Id,
                         BankAccountNumber = c.BankAccountNumber,
                         BankIFSCCode = c.BankIFSCCode,
                         BankName = c.BankName,
                         BankAccountHolder = c.BankAccountHolder,

                     }).ToList()
                 })

             .ToListAsync();

                if (contact == null || contact.Count == 0)
                {
                    response.Message = "No contact found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "contact retrieved successfully";
                    response.HttpResponse = contact;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetContactById(Guid id)
        {

            ClientResponse response = new();
            try
            {
                var contact = await _context.Contacts
                 .Where(x => x.IsDeleted != true).Include(x => x.ContactAdressDetails)
                 .Select(x => new ContactDTO
                 {
                     Id = x.Id,
                     WorkZipCode = x.WorkZipCode,
                     WorkCity = x.WorkCity,
                     WorkStateId = x.WorkStateId,
                     WorkCountryId = x.WorkCountryId,
                     EmployeeId = x.EmployeeId,


                     ContactAdressDetails = x.ContactAdressDetails.Select(c => new ContactAdressDTO
                     {
                         Id = c.Id,
                         Number = c.Number,
                         Street = c.Street,
                         ContactStateId = c.ContactStateId,
                         ContactCountryId = c.ContactCountryId,
                         ContactZipCode = c.ContactZipCode,
                         ContactCity = c.ContactCity,
                         ContactPhone1 = c.ContactPhone1,
                         ContactPhone2 = c.ContactPhone2,
                         ContactEmailPrivate = c.ContactEmailPrivate,
                         ContactEmailbeON = c.ContactEmailbeON,
                         ContactEntitlement = c.ContactEntitlement,

                     }).ToList(),

                     BankDetails = x.Bankdetails.Select(c => new BankDetailsDTO
                     {
                         Id = c.Id,
                         BankAccountNumber = c.BankAccountNumber,
                         BankIFSCCode = c.BankIFSCCode,
                         BankName = c.BankName,
                         BankAccountHolder = c.BankAccountHolder,

                     }).ToList()
                 })

             .ToListAsync();

                if (contact == null || contact.Count == 0)
                {
                    response.Message = "No contact found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "contact retrieved successfully";
                    response.HttpResponse = contact;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }

}