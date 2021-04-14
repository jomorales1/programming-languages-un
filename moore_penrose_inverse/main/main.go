package main

import (
    "fmt"
    "gonum.org/v1/gonum/mat"
)

func main() {
    // Read matrix, then calculate pseudoinverse
    fmt.Println("Moore-Penrose Inverse")

    fmt.Print("Ingrese la primera dimensión de la matriz (m): ")
    var m int
    fmt.Scanln(&m)

    fmt.Print("Ingrese la segunda dimensión de la matriz (n): ")
    var n int
    fmt.Scanln(&n)

    fmt.Println("Ingrese los valores de la matriz:")
    data := make([]float64, m * n)
    for i := 0; i < m * n; i++ {
        fmt.Scan(&data[i])
    }

    A := mat.NewDense(m, n, data)
    A_T := A.T()
    AA_T := mat.NewDense(n, n, nil)
    AA_T.Mul(A_T, A)
    AA_T_I := mat.NewDense(n, n, nil)
    AA_T_I.Inverse(AA_T)
    A_plus := mat.NewDense(n, m, nil)
    A_plus.Mul(AA_T_I, A_T)

    fmt.Println("\nPseudoinversa A+ es igual a: ")
    result := mat.Formatted(A_plus, mat.Prefix(""), mat.Squeeze())
    fmt.Printf("%v\n", result)
}