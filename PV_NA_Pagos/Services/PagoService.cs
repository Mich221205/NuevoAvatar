using PV_NA_Pagos.Entities;
using PV_NA_Pagos.Repository;
using System.Net.Http.Json;

namespace PV_NA_Pagos.Services
{
    public class PagoService
    {
        private readonly PagoRepository _repository;
        private readonly HttpClient _bitacoraClient;

        public PagoService(PagoRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _bitacoraClient = httpClientFactory.CreateClient("BitacoraClient");
        }

        public async Task<Pago?> CrearAsync(int idFactura, decimal monto, int idUsuario)
        {
            var pago = new Pago
            {
                ID_Factura = idFactura,
                Monto = monto,
                Estado = "Aplicado",
                FechaPago = DateTime.Now
            };

            int idPago = await _repository.CrearAsync(pago);

            await RegistrarBitacoraAsync(idUsuario, $"El usuario {idUsuario} registró el pago {idPago} para la factura {idFactura} por ₡{monto}.");

            return await _repository.ObtenerPorIdAsync(idPago);
        }

        public async Task<Pago?> ObtenerPorIdAsync(int id)
            => await _repository.ObtenerPorIdAsync(id);

        public async Task<IEnumerable<Pago>> ListarAsync()
            => await _repository.ListarAsync();

        public async Task<bool> ReversarAsync(int id, int idUsuario)
        {
            bool ok = await _repository.ReversarAsync(id);
            if (ok)
                await RegistrarBitacoraAsync(idUsuario, $"El usuario {idUsuario} anuló el pago {id}.");
            return ok;
        }

        private async Task RegistrarBitacoraAsync(int idUsuario, string accion)
        {
            try
            {
                await _bitacoraClient.PostAsJsonAsync("/bitacora", new
                {
                    ID_Usuario = idUsuario,
                    Accion = accion
                });
            }
            catch
            {
               
            }
        }
    }
}
