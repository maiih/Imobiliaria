using Microsoft.AspNetCore.Mvc;
using ImobiliariaMVC.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImobiliariaMVC.Controllers
{
    public class ImovelController : Controller
    {
        private static List<Imovel> imoveis = new();

        public IActionResult Index()
        {
            return View(imoveis);
        }

        public IActionResult Create()
        {
            ViewBag.TiposComodo = Enum.GetValues(typeof(TipoComodo))
                .Cast<TipoComodo>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = t.ToString()
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Imovel imovel, IFormFile imagem)
        {
            if (imagem != null && imagem.Length > 0)
            {
                var pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(pastaImagens))
                    Directory.CreateDirectory(pastaImagens);

                var nomeArquivo = $"{Guid.NewGuid()}_{imagem.FileName}";
                var caminhoCompleto = Path.Combine(pastaImagens, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                imovel.Imagem = "/images/" + nomeArquivo;
            }

            imovel.Id = imoveis.Count + 1;
            imoveis.Add(imovel);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var imovel = imoveis.FirstOrDefault(i => i.Id == id);
            if (imovel == null)
                return RedirectToAction("Index");

            return View(imovel);
        }

        public IActionResult Edit(int id)
        {
            var imovel = imoveis.FirstOrDefault(i => i.Id == id);
            if (imovel == null)
                return RedirectToAction("Index");

            ViewBag.TiposComodo = Enum.GetValues(typeof(TipoComodo))
                .Cast<TipoComodo>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = t.ToString()
                }).ToList();

            return View(imovel);
        }

        [HttpPost]
        public IActionResult Edit(Imovel model, IFormFile imagem)
        {
            var imovel = imoveis.FirstOrDefault(i => i.Id == model.Id);
            if (imovel == null)
                return RedirectToAction("Index");

            imovel.Titulo = model.Titulo;
            imovel.Descricao = model.Descricao;
            imovel.Categoria = model.Categoria;
            imovel.TipoNegocio = model.TipoNegocio;
            imovel.Valor = model.Valor;
            imovel.Endereco = model.Endereco;
            imovel.Comodos = model.Comodos;

            if (imagem != null && imagem.Length > 0)
            {
                var pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(pastaImagens))
                    Directory.CreateDirectory(pastaImagens);

                var nomeArquivo = $"{Guid.NewGuid()}_{imagem.FileName}";
                var caminhoCompleto = Path.Combine(pastaImagens, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                imovel.Imagem = "/images/" + nomeArquivo;
            }

            return RedirectToAction("Details", new { id = imovel.Id });
        }

        public IActionResult Search(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return View("Index", imoveis);

            termo = termo.Trim().ToLower();

            var resultado = imoveis.Where(i =>

                (!string.IsNullOrEmpty(i.Titulo) &&
                 i.Titulo.ToLower().Contains(termo))

                ||

                (!string.IsNullOrEmpty(i.Descricao) &&
                 i.Descricao.ToLower().Contains(termo))

                ||

                (!string.IsNullOrEmpty(i.Endereco) &&
                 i.Endereco.ToLower().Contains(termo))

                ||

                i.Categoria.ToString().ToLower().Contains(termo)

                ||

                i.TipoNegocio.ToString().ToLower().Contains(termo)

                ||

                i.Valor.ToString().Contains(termo)

                ||

                (i.Comodos != null &&
                 i.Comodos.Any(c =>
                     c.ToString().ToLower().Contains(termo)))

            ).ToList();

            return View("Index", resultado);
        }
    }
}