namespace tprevision.Models.ModelTemplate
{
    public class ProduitSansNavigation
    {
        private int idProduit;
        private string? nomProduit;
        private string? description;
        private string? nomPhoto;
        private string? uriPhoto;
        private int idTypeProduit;
        private int idMarque;
        private int stockReel;
        private int stockMin;
        private int stockMax;

        public int IdProduit
        {
            get
            {
                return idProduit;
            }

            set
            {
                idProduit = value;
            }
        }

        public string? NomProduit
        {
            get
            {
                return nomProduit;
            }

            set
            {
                nomProduit = value;
            }
        }

        public string? Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public string? NomPhoto
        {
            get
            {
                return nomPhoto;
            }

            set
            {
                nomPhoto = value;
            }
        }

        public string? UriPhoto
        {
            get
            {
                return uriPhoto;
            }

            set
            {
                uriPhoto = value;
            }
        }

        public int IdTypeProduit
        {
            get
            {
                return idTypeProduit;
            }

            set
            {
                idTypeProduit = value;
            }
        }

        public int IdMarque
        {
            get
            {
                return idMarque;
            }

            set
            {
                idMarque = value;
            }
        }

        public int StockReel
        {
            get
            {
                return stockReel;
            }

            set
            {
                stockReel = value;
            }
        }

        public int StockMin
        {
            get
            {
                return stockMin;
            }

            set
            {
                stockMin = value;
            }
        }

        public int StockMax
        {
            get
            {
                return this.stockMax;
            }

            set
            {
                this.stockMax = value;
            }
        }
    }
}
