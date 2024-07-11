using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly FiapDbContext _context;

        public PagamentoRepository(FiapDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento?> GetPagamentoByIdPedido(int idPedido)
        {
            if (_context.Pagamento is null)
                throw new InvalidOperationException("Contexto de pagamento nulo.");

            // Caso encontre mais de um registro pega sempre o ultimo.
            return await _context.Pagamento.Where(p => p.IdPedido == idPedido)
                                           .OrderByDescending(p => p.DataPagamento)
                                           .FirstOrDefaultAsync();
        }

        public async Task<Pagamento> PostPagamento(Pagamento pagamento)
        {
            if (_context.Pagamento is not null)
            {
                _context.Pagamento.Add(pagamento);
                await _context.SaveChangesAsync();
            }
            return pagamento;
        }

        public async Task<Pagamento> PutPagamento(Pagamento pagamento)
        {
            if (_context.Pagamento is not null)
            {
                var pagto = await _context.Pagamento.FirstOrDefaultAsync(p => p.IdPagamento == pagamento.IdPagamento);
                pagto.StatusPagamento = pagamento.StatusPagamento;
                _context.Pagamento.Update(pagto);
                await _context.SaveChangesAsync();
            }
            return pagamento;
        }
    }
}
