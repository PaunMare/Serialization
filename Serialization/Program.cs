using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person("Random", 25);
            Serialize(p);

            Person person = Deserialize();
            Console.WriteLine(person.ToString());

            Console.ReadLine();
            
        }
        public static void Serialize(Person p)
        {
            List<byte> allBytes = new List<byte>();
            byte[] bufferInt = new byte[4];
            byte[] bufferChar = new byte[2];
            byte[] help; 
            WriteInt32(ref bufferInt, p.Age, 0);
            FromArrayToList(allBytes, bufferInt);
            foreach (char c in p.Name)
            {
                WriteChar16(ref bufferChar, c, 0);
                FromArrayToList(allBytes, bufferChar);
            }
            help = ReturnByteArray(allBytes);
            File.WriteAllBytes("Broj", help);
        }
        public static Person Deserialize()
        {
            byte[] help = File.ReadAllBytes("Broj");
            List<char> chars = new List<char>();
            Person p = new Person();
            p.Age = ReadInt32(help, 0);
            int offset = sizeof(int);
            for (int i = offset; i < help.Length; i += sizeof(char))
            {
                chars.Add(ReadChar16(help, i));
            }
            p.Name = new string(chars.ToArray());

            return p;

        }
        static void FromArrayToList(List<byte> list,byte[] array)
        {
            foreach(byte b in array)
            {
                list.Add(b);
            }
        }
        static byte[] ReturnByteArray(List<byte> bytes)
        {
            byte[] array = bytes.ToArray();
            return array;
        }
        static unsafe void WriteInt32(ref byte[] buffer, int value, int offset)
        {
            fixed (byte* ptr = buffer)
            {
                *(int*)(ptr + offset) = value;
            }

        }
        static unsafe int ReadInt32(byte[] buffer,int offset)
        {
            fixed(byte* ptr = &buffer[offset])
            {
                return *((int*)ptr);
            }
        }
        static unsafe char ReadChar16(byte[] buffer, int offset)
        {
            fixed (byte* ptr = &buffer[offset])
            {
                return *((char*)ptr);
            }
        }
        static unsafe void WriteChar16(ref byte[] buffer, char c, int offset)
        {
            fixed (byte* ptr = buffer)
            {
                *(char*)(ptr + offset) = c;
            }
        } 
        
        public class Person
        {
            string name;
            int age;
            public Person()
            {
                name = "";
                age = 0;
            }
            public Person(string _name, int _age)
            {
                name = _name;
                age = _age;
            }
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public int Age
            {
                get { return age; }
                set { age = value; }
            }

            public override string ToString()
            {
                return "Name: " + name + ", Age: " + age;
            }
        }


    }
}
