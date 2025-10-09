using PV_NA_Pagos.Entities;
using PV_NA_Pagos.Repository;
using System.Net.Http.Json;

namespace PV_NA_Pagos.Services
{
    public class FacturaService
    {
        private readonly FacturaRepository _repository;
        private readonly HttpClient _bitacoraClient;

        public FacturaService(FacturaRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _bitacoraClient = httpClientFactory.CreateClient("BitacoraClient");
        }

        public async Task<Factura?> CrearFacturaAsync(int idEstudiante, int idUsuario)
        {
        
            const decimal precioCurso = 30000m;
            decimal impuesto = Math.Round(precioCurso * 0.02m, 2);

            var factura = new Factura
            {
                ID_Estudiante = idEstudiante,
                Monto = precioCurso,
                Impuesto = impuesto,
                Estado = "Pendiente",
                Fecha = DateTime.Now
            };

            int idFactura = await _repository.CrearAsync(factura);
            await _repository.CrearDetalleAsync(idFactura);

          
            var accion = $"El usuario {idUsuario} creó la factura {idFactura} para el estudiante {idEstudiante}.";
            await RegistrarBitacoraAsync(idUsuario, accion);

            return await _repository.ObtenerPorIdAsync(idFactura);
        }

        public async Task<Factura?> ObtenerPorIdAsync(int idFactura)
        {
            return await _repository.ObtenerPorIdAsync(idFactura);
        }

        public async Task<IEnumerable<Factura>> ListarAsync()
        {
            return await _repository.ListarAsync();
        }

        public async Task<bool> ReversarAsync(int idFactura, int idUsuario)
        {
            bool ok = await _repository.ReversarAsync(idFactura);
            if (ok)
            {
                var accion = $"El usuario {idUsuario} reversó la factura {idFactura}.";
                await RegistrarBitacoraAsync(idUsuario, accion);
            }
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
