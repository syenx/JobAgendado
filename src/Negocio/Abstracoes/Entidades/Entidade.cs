using System;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades
{
    public abstract class Entidade : IEntidade
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }

        public override bool Equals(object outra)
        {
            if (outra == null || !GetType().Equals(outra.GetType()))
                return false;

            return Equals((Entidade)outra);
        }

        public bool Equals(Entidade outra)
        {
            return outra.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(Entidade).FullName, Id);
        }
    }
}
