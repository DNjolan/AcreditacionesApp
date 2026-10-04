namespace AcreditacionesApp.Api.Services;

// Contrato: "Alguien que sabe qu[e d[ia es hoy".
public interface IReloj
{
    DateOnly Hoy { get; }       // la interfaz exige una propiedad Hoy con get
}
