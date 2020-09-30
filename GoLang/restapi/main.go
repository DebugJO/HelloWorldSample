package main

import (
	"encoding/json"
	"log"
	"math/rand"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
)

// Book Model
type Book struct {
	ID     string    `json:"id"`
	Isbn   string    `json:"isbn"`
	Title  string    `json:"title"`
	Author *[]Author `json:"author"`
}

// Author Model
type Author struct {
	Name    string `json:"name"`
	Address string `json:"address"`
}

var books []Book

func setBook(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	var book Book
	_ = json.NewDecoder(r.Body).Decode(&book)
	book.ID = strconv.Itoa(rand.Intn(100000000))
	books = append(books, book)
	json.NewEncoder(w).Encode(book)
}

func main() {
	r := mux.NewRouter()

	r.HandleFunc("/books", setBook).Methods("POST")

	log.Fatal(http.ListenAndServe(":8000", r))
}
