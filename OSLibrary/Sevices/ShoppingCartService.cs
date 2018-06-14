using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Containers;
using OSLibrary.Models;
using OSLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Sevices
{
    public class ShoppingCartService
    {
        public IEnumerable<ShoppingCartDetail> GetAccountCart(string Account)
        {
            var model = new List<ShoppingCartDetail>();
            ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository();
            ProductsRepository productsRepository = new ProductsRepository();
            ProductImageRepository productImageRepository = new ProductImageRepository();

            foreach (var item in shoppingCartRepository.GetByAccount(Account))
            {
                var cart = new ShoppingCartDetail
                {
                    Account = Account,
                    Color = item.Color,
                    ProductID = item.Product_ID,
                    Name = productsRepository.GetByProduct_ID(item.Product_ID).Product_Name,
                    ShoppingCartID = item.Shopping_Cart_ID,
                    Quantity = item.Quantity,
                    RowPrice = item.UnitPrice * item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Size = item.size,
                    imgurl = productImageRepository.GetByProduct_ID(item.Product_ID).FirstOrDefault(x=>x.Image_Only=="YES").Image
                };
                model.Add(cart);
            }
            return model;
        }
        public bool CreateShoppingCart(string _account, int Product_ID, int Quantity, string Size, string Color)
        {
            //宣告 庫存,購物車,產品 的 Rpository 
            StockRepository sizeQuantityRepository = RepositoryContainer.GetInstance<StockRepository>();
            ShoppingCartRepository shoppingCart = RepositoryContainer.GetInstance<ShoppingCartRepository>();
            ProductsRepository products = RepositoryContainer.GetInstance<ProductsRepository>();

            //先取得那項產品庫存 放到 stock
            var stock = sizeQuantityRepository.GetByPK(Product_ID, Size, Color);
            //如果庫存量少與需求量 回傳 false 
            if (stock.Quantity < Quantity)
            {
                return false;
            }
            //取得 目前 Account 的 購物車
            var myCart = shoppingCart.GetByAccount(_account);
            //查看購物車是否有那項產品
            //如果有 把需要的數量 新增 進原有的購物車的裡面
            var items = myCart.FirstOrDefault(x => (x.Product_ID == Product_ID) && (x.size == Size)&&(x.Color == Color));
            if (items != null)
            {
                //去更新購物車的那項產品
                shoppingCart.Update(items.Shopping_Cart_ID, items.Quantity + Quantity);
            }
            else
            {
                //如果購物車沒有那項產品就新增一項產品進到購物車
                var model = new Shopping_Cart()
                {
                    Account = _account,
                    Product_ID = Product_ID,
                    size = Size,
                    UnitPrice = products.GetByProduct_ID(Product_ID).UnitPrice,
                    Quantity = (short)Quantity,
                    Color = Color,
                    
                };
                shoppingCart.Create(model);
            }
            return true;
        }
        public bool Delete_ProductOfCart(int ShoppingCartID)
        {
            var cart = RepositoryContainer.Container.GetInstance<ShoppingCartRepository>();
            try
            {
                cart.Delete(ShoppingCartID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeProductofCart(int shoppingCartID,int quantity)
        {
            ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository();
            try
            {
                shoppingCartRepository.Update(shoppingCartID, quantity);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
