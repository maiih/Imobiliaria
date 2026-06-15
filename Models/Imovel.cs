namespace ImobiliariaMVC.Models
{
    public enum CategoriaImovel
    {
        Comercial,
        Residencial,
        Rural
    }

    public enum TipoNegocio
    {
        Venda,
        Aluguel
    }

    public class Imovel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string?Categoria { get; set; }
        public string?TipoNegocio { get; set; }
        public decimal Valor { get; set; }
        public string?Endereco { get; set; }
        public string?Imagem { get; set; }

        public List<Comodo> Comodos { get; set; } = new();
    }
}
