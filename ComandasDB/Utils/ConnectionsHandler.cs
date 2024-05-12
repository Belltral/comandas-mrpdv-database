using ComandasDB.Data;
using ComandasDB.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComandasDB
{
    internal class ConnectionsHandler
    {
        private static TcpListener _listener = null;
        private static byte[] dataToSend;

        internal static async Task<object> ClientConnection(string ipAddress, Request request, string comandaJson = null)
        {
            if (!(comandaJson is null))
            {
                request.BufferSize = Encoding.UTF8.GetBytes(comandaJson).Length;
            }

            int portNumber = 8130;

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(IPAddress.Parse(ipAddress), portNumber);

                    using (NetworkStream stream = client.GetStream())
                    {
                        string sendRequest = JsonConvert.SerializeObject(request);
                        byte[] requestToSend = Encoding.UTF8.GetBytes(sendRequest);

                        switch (request.RequestType)
                        {
                            case RequestTypes.DataRequest:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                // Resposta da solicitação de buffer para o servidor
                                var requestSizeBuffer = new byte[1024];
                                int bytesSizeRead = await stream.ReadAsync(requestSizeBuffer, 0, requestSizeBuffer.Length);

                                int requestSize = int.Parse(Encoding.UTF8.GetString(requestSizeBuffer));

                                var bufferToReceiveRequest = new byte[requestSize];

                                // Envia a confirmação para o servidor
                                byte[] ackSend = Encoding.UTF8.GetBytes("ACK");
                                await stream.WriteAsync(ackSend, 0, ackSend.Length);

                                // Recebe a requisição e faz a desserialização
                                int data = await stream.ReadAsync(bufferToReceiveRequest, 0, bufferToReceiveRequest.Length);
                                string requestReceived = Encoding.UTF8.GetString(bufferToReceiveRequest);

                                if (request.IsPreVenda)
                                {
                                    PreVenda preVenda = JsonConvert.DeserializeObject<PreVenda>(requestReceived);

                                    return preVenda;
                                }

                                else if (request.IsItensPreVendas)
                                {
                                    List<ItensPreVenda> itensPreVenda = JsonConvert.DeserializeObject<List<ItensPreVenda>>(requestReceived);

                                    return itensPreVenda;
                                }

                                else
                                {
                                    Comanda comandaDeserialized = JsonConvert.DeserializeObject<Comanda>(requestReceived);

                                    return comandaDeserialized;
                                }

                            case RequestTypes.DataSave:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                // Aguarda o servidor responder que armazenou o buffer
                                var serverSavedBuffer = new byte[1024];
                                await stream.ReadAsync(serverSavedBuffer, 0, serverSavedBuffer.Length);
                                var serverAck = Encoding.UTF8.GetString(serverSavedBuffer);

                                // Envia os dados a serem salvos
                                var comandaBuffer = Encoding.UTF8.GetBytes(comandaJson);
                                await stream.WriteAsync(comandaBuffer, 0, comandaBuffer.Length);

                                // Aguarda a resposta da requisição
                                var savedBuffer = new byte[1024];
                                await stream.ReadAsync(savedBuffer, 0, savedBuffer.Length);
                                bool isComandaSaved = bool.Parse(Encoding.UTF8.GetString(savedBuffer));

                                return isComandaSaved;
                            
                            case RequestTypes.DataUpdate:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var serverUpdateBufferAck = new byte[1024];
                                await stream.ReadAsync(serverUpdateBufferAck, 0, serverUpdateBufferAck.Length);
                                var serverUpdateAck = Encoding.UTF8.GetString(serverUpdateBufferAck);

                                var updateComandaBuffer = Encoding.UTF8.GetBytes(comandaJson);
                                await stream.WriteAsync(updateComandaBuffer, 0, updateComandaBuffer.Length);

                                var updateBuffer = new byte[1024];
                                await stream.ReadAsync(updateBuffer, 0, updateBuffer.Length);
                                bool updateComandaResponse = bool.Parse(Encoding.UTF8.GetString(updateBuffer));

                                return updateComandaResponse;

                            case RequestTypes.DataDelete:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var deleteBuffer = new byte[1024];
                                await stream.ReadAsync(deleteBuffer, 0, deleteBuffer.Length);
                                var deleteResponse = bool.Parse(Encoding.UTF8.GetString(deleteBuffer));

                                return deleteResponse;

                            case RequestTypes.DataDeleteAll:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var deleteAllBuffer = new byte[1024];
                                await stream.ReadAsync(deleteAllBuffer, 0, deleteAllBuffer.Length);
                                var deleteAllResponse = bool.Parse(Encoding.UTF8.GetString(deleteAllBuffer));

                                return deleteAllResponse;

                            case RequestTypes.DataCheck:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var checkBuffer = new byte[1024];
                                await stream.ReadAsync(checkBuffer, 0, checkBuffer.Length);
                                var checkResponse = bool.Parse(Encoding.UTF8.GetString(checkBuffer));

                                return checkResponse;
                        }
                    }
                }
                catch (SocketException e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                }
                catch (JsonException e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                    await Console.Out.WriteLineAsync(e.Source);
                }
                return null;
            }
            
        }

        private static async Task InformBufferSize(NetworkStream stream, Request request)
        {
            try
            {
                if (request.IsPreVenda)
                {
                    // Localizar Pre Venda, serializar e encodar
                    var preVenda = ComandasHandler.GetPreVenda(request.ComandaNumber);
                    string preVendaJson = JsonConvert.SerializeObject(preVenda);
                    dataToSend = Encoding.UTF8.GetBytes(preVendaJson);

                    string preVendaBufferSize = dataToSend.Length.ToString();
                    byte[] preVendaBuffer = Encoding.UTF8.GetBytes(preVendaBufferSize);

                    await stream.WriteAsync(preVendaBuffer, 0, preVendaBuffer.Length);

                    return;
                }

                else if (request.IsItensPreVendas)
                {
                    var itensPreVendas = ComandasHandler.GetItensComanda(request.ComandaNumber);
                    string itensJson = JsonConvert.SerializeObject(itensPreVendas);
                    dataToSend = Encoding.UTF8.GetBytes(itensJson);

                    string itensBufferSize = dataToSend.Length.ToString();
                    byte[] itensBuffer = Encoding.UTF8.GetBytes(itensBufferSize);

                    await stream.WriteAsync(itensBuffer, 0, itensBuffer.Length);

                    return;
                }

                Comanda comanda = ComandasHandler.GetComanda(request.ComandaNumber);
                string comandaJson = JsonConvert.SerializeObject(comanda);
                dataToSend = Encoding.UTF8.GetBytes(comandaJson);

                string comandaBufferSize = dataToSend.Length.ToString();
                byte[] comandaBuffer = Encoding.UTF8.GetBytes(comandaBufferSize);

                await stream.WriteAsync(comandaBuffer, 0, comandaBuffer.Length);
            
            }
            catch (JsonException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                await Console.Out.WriteLineAsync(e.StackTrace);
            }
            catch (SocketException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                await Console.Out.WriteLineAsync(e.StackTrace);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                await Console.Out.WriteLineAsync(e.StackTrace);
            }
            
        }

        internal static async Task ServerConnection()
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8130);
                _listener = new TcpListener(iPEndPoint);

                _listener.Start();

                while (true)
                {
                    using (TcpClient handler = await _listener.AcceptTcpClientAsync())
                    {
                        using (NetworkStream stream = handler.GetStream())
                        {
                            try
                            {
                                // Ler dados recebidos
                                var buffer = new byte[1024];
                                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                                string dataReceived = Encoding.UTF8.GetString(buffer);
                                Request request = JsonConvert.DeserializeObject<Request>(dataReceived);

                                switch (request.RequestType)
                                {
                                    case RequestTypes.DataRequest:
                                        await InformBufferSize(stream, request);

                                        // Aguarda pela confirmação de alocação de memória
                                        var clientSavedBuffer = new byte[512];
                                        await stream.ReadAsync(clientSavedBuffer, 0, clientSavedBuffer.Length);
                                        var clientAck = Encoding.UTF8.GetString(clientSavedBuffer);

                                        // Envia os dados
                                        await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
                                        break;

                                    case RequestTypes.DataSave:
                                        var saveBuffer = new byte[request.BufferSize];

                                        // Envia a confirmação para o cliente
                                        byte[] saveAckSend = Encoding.UTF8.GetBytes("ACK");
                                        await stream.WriteAsync(saveAckSend, 0, saveAckSend.Length);

                                        // Recebe os dados para serem salvos
                                        await stream.ReadAsync(saveBuffer, 0, saveBuffer.Length);

                                        var decodedComandaSave = Encoding.UTF8.GetString(saveBuffer);
                                        Comanda comandaToSave = JsonConvert.DeserializeObject<Comanda>(decodedComandaSave);

                                        var isComandaSaved = ComandasHandler.SaveComanda(comandaToSave);
                                        var isComandaSavedEncoded = Encoding.UTF8.GetBytes(isComandaSaved.ToString());
                                        await stream.WriteAsync(isComandaSavedEncoded, 0, isComandaSavedEncoded.Length);

                                        break;

                                    case RequestTypes.DataUpdate:
                                        var updateBuffer = new byte[request.BufferSize];

                                        byte[] updateAckSend = Encoding.UTF8.GetBytes("ACK");
                                        await stream.WriteAsync(updateAckSend, 0, updateAckSend.Length);

                                        await stream.ReadAsync(updateBuffer, 0, updateBuffer.Length);

                                        var decodedComandaUpdate = Encoding.UTF8.GetString(updateBuffer);
                                        Comanda comandaToUpdate = JsonConvert.DeserializeObject<Comanda>(decodedComandaUpdate);

                                        bool updatedComandaCheck = ComandasHandler.UpdateComanda(comandaToUpdate);
                                        var encodedUpdateCheck = Encoding.UTF8.GetBytes(updatedComandaCheck.ToString());
                                        await stream.WriteAsync(encodedUpdateCheck, 0, encodedUpdateCheck.Length);

                                        break;

                                    case RequestTypes.DataDelete:
                                        bool deletedComandas = ComandasHandler.DeleteComanda(request.ComandaNumber);
                                        var encodedDRespose = Encoding.UTF8.GetBytes(deletedComandas.ToString());
                                        await stream.WriteAsync(encodedDRespose, 0, encodedDRespose.Length);

                                        break;

                                    case RequestTypes.DataDeleteAll:
                                        bool deletedAllComandas = ComandasHandler.DeleteAllComandas();
                                        var encodedDaRespose = Encoding.UTF8.GetBytes(deletedAllComandas.ToString());
                                        await stream.WriteAsync(encodedDaRespose, 0, encodedDaRespose.Length);

                                        break;

                                    case RequestTypes.DataCheck:
                                        bool dataCheck = ComandasHandler.ComandaExistis(request.ComandaNumber);
                                        var encodedCheckRespose = Encoding.UTF8.GetBytes(dataCheck.ToString());
                                        await stream.WriteAsync(encodedCheckRespose, 0, encodedCheckRespose.Length);
                                        break;
                                }
                            }
                            catch (SocketException e)
                            {
                                await Console.Out.WriteLineAsync(e.Message);
                                await Console.Out.WriteLineAsync(e.StackTrace);
                            }
                            catch (JsonException e)
                            {
                                await Console.Out.WriteLineAsync(e.Message);
                                await Console.Out.WriteLineAsync(e.StackTrace);
                            }
                            catch (Exception e)
                            {
                                await Console.Out.WriteLineAsync(e.Message);
                                await Console.Out.WriteLineAsync(e.StackTrace);
                                await Console.Out.WriteLineAsync(e.Source);
                            }
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                await Console.Out.WriteLineAsync(e.StackTrace);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                await Console.Out.WriteLineAsync(e.StackTrace);
                await Console.Out.WriteLineAsync(e.Source);
            }
        }

        internal static void StopServer()
        {
            try {
                if (_listener != null)
                {
                    _listener.Stop();
                }
            }
            catch (SocketException e) { }
        }
    }
}
