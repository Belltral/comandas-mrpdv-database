﻿using ComandasDB.Data;
using ComandasDB.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComandasDB
{
    /// <summary>
    /// Classe de implementação no cliente.
    /// <para>Essa classe faz a interface com os componentes mais baixos para simplificar a implementação.</para>
    /// </summary>
    public class Comandas
    {
        public static string IPAddress = "192.168.0.10";

        /// <summary>
        /// Retorna uma comanda do banco
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser retornada</param>
        /// <returns></returns>
        public static async Task<Comanda> GetComanda(int comandaNumber)
        {
            Request request = new Request()
            {
                RequestType = RequestTypes.DataRequest,
                IsPreVenda = false,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
            };

            var comanda = await ConnectionsHandler.ClientConnection(IPAddress, request);//.GetAwaiter().GetResult(); //.Result

            return (Comanda)comanda;
        }

        /// <summary>
        /// Retorna uma Pré Venda do banco
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual pertence a Pré Venda</param>
        /// <returns></returns>
        public static async Task<PreVenda> GetPreVenda(int comandaNumber)
        {
            Request request = new Request
            {
                RequestType = RequestTypes.DataRequest,
                IsPreVenda = true,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
            };

            var preVenda = await ConnectionsHandler.ClientConnection(IPAddress, request);

            return (PreVenda)preVenda;
        }

        /// <summary>
        /// Retorna os produtos de uma comanda específica
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual os produtos fazem parte</param>
        /// <returns></returns>
        public static async Task<List<ItensPreVenda>> GetItensPreVenda(int comandaNumber)
        {
            Request request = new Request()
            {
                RequestType = RequestTypes.DataRequest,
                IsPreVenda = false,
                IsItensPreVendas = true,
                ComandaNumber = comandaNumber,
            };

            var itensPreVenda = await ConnectionsHandler.ClientConnection(IPAddress, request);

            return (List<ItensPreVenda>)itensPreVenda;
        }

        /// <summary>
        /// Verifica se uma comanda já existe na base
        /// </summary>
        /// <param name="comandaNumber">Número da comanda para verificar sua existência</param>
        /// <returns>Retorna verdadeiro caso exista uma comanda com o mesmo número ou falso caso não sejam encontrados dados</returns>
        public static async Task<bool> ComandaExistis(int comandaNumber)
        {
            Request request = new Request()
            {
                RequestType = RequestTypes.DataCheck,
                IsPreVenda = false,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
            };

            var check = await ConnectionsHandler.ClientConnection(IPAddress, request);

            return (bool)check;
        }

        /// <summary>
        /// Salva uma comanda na base
        /// </summary>
        /// <param name="comanda">Comanda que será salva</param>
        /// <returns>Retorna verdadeiro caso a comanda seja salva ou falso caso não seja</returns>
        public static async Task<bool> SaveComanda(Comanda comanda)
        {
            string comandaJson = JsonConvert.SerializeObject(comanda);

            Request request = new Request()
            {
                RequestType = RequestTypes.DataSave,
                IsPreVenda = false,
                IsItensPreVendas = false,
            };

            var isComandaSaved = await ConnectionsHandler.ClientConnection(IPAddress, request, comandaJson);

            return (bool)isComandaSaved;
        }
        
        /// <summary>
        /// Faz a atualização de uma comanda existente na base
        /// </summary>
        /// <param name="comanda">Comanda atualizada a ser salva</param>
        /// <returns></returns>
        public static async Task<bool> UpdateComanda(Comanda comanda)
        {
            string comandaJson = JsonConvert.SerializeObject(comanda);

            Request request = new Request()
            {
                RequestType = RequestTypes.DataUpdate,
                IsPreVenda = false,
                IsItensPreVendas = false,
            };

            var update = await ConnectionsHandler.ClientConnection(IPAddress, request, comandaJson);

            return (bool)update;
        }

        /// <summary>
        /// Remove uma comanda da base de acordo com o número
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser removida</param>
        /// <returns>Retorna verdadeiro se o procedimento foi efetuado ou falso caso não exista a comanda informada</returns>
        public static async Task<bool> DeleteComanda(int comandaNumber)
        {
            Request request = new Request()
            {
                RequestType = RequestTypes.DataDelete,
                IsPreVenda = false,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber
            };

            var delete = await ConnectionsHandler.ClientConnection(IPAddress, request);

            return (bool)delete;
        }

        /// <summary>
        /// Apaga todas as comandas da base
        /// </summary>
        /// <returns>Retorna verdadeiro caso as comandas forem apagadas</returns>
        public static async Task<bool> DeleteAllComandas()
        {
            Request request = new Request()
            {
                RequestType = RequestTypes.DataDeleteAll,
                IsPreVenda = false,
                IsItensPreVendas = false,
            };

            var deleteAll = await ConnectionsHandler.ClientConnection(IPAddress, request);

            return (bool)deleteAll;
        }

        /// <summary>
        /// Finaliza a escuta do servidor que aceita as requisições dos clientes para a base de comandas.
        /// <para>Deve ser implementado ao finalizar o PDV manualmente.</para>
        /// </summary>
        public static void CloseServerConnection()
        {
            ConnectionsHandler.StopServer();
        }

        /// <summary>
        /// Inicia o servidor de comandas para escutar as requisições
        /// </summary>
        public static async Task InitiateServer()
        {
            await ConnectionsHandler.ServerConnection();
        }
    }
}