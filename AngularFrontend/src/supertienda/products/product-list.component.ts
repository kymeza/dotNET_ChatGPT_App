import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from './product.service'; // Adjust path as necessary

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: any[] = [];
  currentPage: number = 1;
  pageSize: number = 20;

  newProduct: any = {
    idArticulo: '',
    idSubCategoria: '',
    producto: '',
    precioUnitario: 0,
    costoUnitario: 0
  };

  // Holds the currently edited product
  editedProduct: any = null;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts(this.currentPage, this.pageSize).subscribe((data) => {
      this.products = data;
    });
  }

  addProduct(): void {
    this.productService.addProduct(this.newProduct).subscribe(() => {
      this.loadProducts();
      this.newProduct = {
        idArticulo: '',
        idSubCategoria: '',
        producto: '',
        precioUnitario: 0,
        costoUnitario: 0
      };
    });
  }

  // Set the edited product data for editing
  startEditing(product: any): void {
    this.editedProduct = { ...product }; // Clone the product to avoid direct mutation
  }

  // Update the product
  saveProduct(): void {
    if (this.editedProduct) {
      this.productService.updateProduct(this.editedProduct.idArticulo, this.editedProduct).subscribe(() => {
        this.loadProducts();
        this.editedProduct = null; // Clear the editing state
      });
    }
  }

  cancelEditing(): void {
    this.editedProduct = null;
  }

  deleteProduct(id: string): void {
    this.productService.deleteProduct(id).subscribe(() => this.loadProducts());
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.loadProducts();
  }

  nextPage(): void {
    this.currentPage++;
    this.loadProducts();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadProducts();
    }
  }
}
