﻿using InsuranceApp.DTO;
using InsuranceApp.Models;
using InsuranceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<PaymentDto> paymentDtos = new List<PaymentDto>();
            var payments = _paymentService.GetAll();
            if (payments.Count > 0)
            {
                foreach (var payment in payments)
                    paymentDtos.Add(ConvertToDto(payment));
                return Ok(paymentDtos);
            }
            return NotFound("No Payment created");
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var payment = _paymentService.Get(id);
            if (payment != null)
                return Ok(ConvertToDto(payment));
            return BadRequest("No such Payment Found");
        }
        [HttpPost]
        public IActionResult Add(PaymentDto paymentDto)
        {
            var payment = ConvertToModel(paymentDto);
            int id = _paymentService.Add(payment);
            if (id != null)
                return Ok(id);
            return BadRequest("Some issue while adding record");
        }
        [HttpPut]
        public IActionResult Update(PaymentDto paymentDto)
        {
            var existingPayment = _paymentService.Check(paymentDto.PaymentId);
            if (existingPayment != null)
            {
                var payment = ConvertToModel(paymentDto);
                var modifiedPayment = _paymentService.Update(payment);
                return Ok(ConvertToDto(modifiedPayment));
            }
            return BadRequest("No such record exists");
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id)
        {
            var paymentToDelete = _paymentService.Check(id);
            if (paymentToDelete != null)
            {
                _paymentService.Delete(paymentToDelete);
                return Ok(paymentToDelete.PaymentId);
            }
            return BadRequest("No such record exists");
        }
        private PaymentDto ConvertToDto(Payment payment)
        {
            return new PaymentDto()
            {
                PaymentId = payment.PaymentId,
                PaymentType = payment.PaymentType,
                Amount = payment.Amount,
                Date= payment.Date,
                Tax = payment.Tax,
                TotalPayment= payment.TotalPayment,

                //Contacts = user.Contacts
            };
        }
        private Payment ConvertToModel(PaymentDto paymentDto)
        {
            return new Payment()
            {
                PaymentId = paymentDto.PaymentId,
                PaymentType = paymentDto.PaymentType,
                Amount = paymentDto.Amount,
                Date = paymentDto.Date,
                Tax = paymentDto.Tax,
                TotalPayment = paymentDto.TotalPayment, 
                IsActive = true,

                //Contacts = userDto.Contacts
            };
        }
    }
}
