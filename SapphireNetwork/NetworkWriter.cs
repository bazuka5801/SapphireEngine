﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lidgren.Network;
using UnityEngine;

namespace SapphireNetwork
{
    public class NetworkWriter : INetworkBuffer
    {
        public NetworkPeer Peer { get; }

        public byte[] Buffer
        {
            get => m_memoryStream.ToArray();
            set
            {
                this.m_memoryStream.SetLength(0);
                this.m_memoryStream.Write(value, 0, value.Length);
                this.Position = value.Length;
            }
        }
        public int Length => this.Buffer.Length;
        public int Position { get; set; } = 0;

        private MemoryStream m_memoryStream = new MemoryStream();

        public NetworkWriter(NetworkPeer peer)
        {
            this.Peer = peer;
        }
        
        public bool Start()
        {
            if (this.Peer.Status)
            {
                this.Buffer = new byte[] {this.Peer.Configuration.IndeficationByte};
                return true;
            }
            return false;
        }
        
        public void Send(List<NetworkConnection> connections)
        {
            if (this.Peer.Configuration.Cryptor != null)
                this.Buffer = this.Peer.Configuration.Cryptor.Encryption(this.Buffer);

            var outMessage = this.Peer.BaseSocket.CreateMessage();
            outMessage.Write(this.Buffer.Length);
            outMessage.Write(this.Buffer);
            
            for (int i = 0; i < connections.Count; ++i)
                if (connections[i].IsConnected)
                    this.Peer.BaseSocket.SendMessage(outMessage, connections[i].Addres, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendTo(NetworkConnection connection)
        {
            if (connection.IsConnected == false)
                return;
            
            if (this.Peer.Configuration.Cryptor != null)
                this.Buffer = this.Peer.Configuration.Cryptor.Encryption(this.Buffer);
            
           
            
            var outMessage = this.Peer.BaseSocket.CreateMessage();
            outMessage.Write(this.Buffer.Length);
            outMessage.Write(this.Buffer);
            
            this.Peer.BaseSocket.SendMessage(outMessage, connection.Addres, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendToAll()
        {
            if (this.Peer.Configuration.Cryptor != null)
                this.Buffer = this.Peer.Configuration.Cryptor.Encryption(this.Buffer);

            var outMessage = this.Peer.BaseSocket.CreateMessage();
            outMessage.Write(this.Buffer.Length);
            outMessage.Write(this.Buffer);

            foreach (var connection in this.Peer.m_listconnections)
                if (connection.Value.IsConnected)
                    this.Peer.BaseSocket.SendMessage(outMessage, connection.Key, NetDeliveryMethod.ReliableOrdered);
        }

        public void Byte(byte arg)
        {
            this.m_memoryStream.WriteByte(arg);
            this.Position += 1;
        }

        public void Boolean(bool arg)
        {
            this.Byte((byte)(arg ? 1 : 0));
        }

        public void Int16(Int16 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void UInt16(UInt16 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Int32(Int32 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void UInt32(UInt32 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Int64(Int64 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void UInt64(UInt64 arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Char(Char arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Double(Double arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Float(Single arg)
        {
            byte[] buffer = BitConverter.GetBytes(arg);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }

        public void Bytes(byte[] arg)
        {
            
            this.m_memoryStream.Write(arg, 0, arg.Length);
            this.Position += arg.Length;
        }
        
        public void String(String arg)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(arg);
            this.UInt16((ushort)buffer.Length);
            this.m_memoryStream.Write(buffer, 0, buffer.Length);
            this.Position += buffer.Length;
        }
        
        public void Vector2(Vector2 arg)
        {
            this.Float(arg.x);
            this.Float(arg.y);
        }

        public void Vector3(Vector3 arg)
        {
            this.Float(arg.x);
            this.Float(arg.y);
            this.Float(arg.z);
        }

        public void Vector4(Vector4 arg)
        {
            this.Float(arg.x);
            this.Float(arg.y);
            this.Float(arg.z);
            this.Float(arg.w);
        }
    }
}