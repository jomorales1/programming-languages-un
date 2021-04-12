# Damerou-Levenshtein distance of a programming language

dldistance <- function(a, b) {
  n1 <- nchar(a)
  n2 <- nchar(b)
  d = matrix(0, nrow = n1 + 1, ncol = n2 + 1)
  for (index in 1:dim(d)[1]) {
    d[index, 1] <- index - 1
  }
  for (index in 1:dim(d)[2]) {
    d[1, index] <- index - 1
  }
  for (i in 2:dim(d)[1]) {
    for (j in 2:dim(d)[2]) {
      cost <- 0
      if (substring(a, i, i) != substring(b, j, j)) {
        cost <- 1
      }
      options = c(d[i - 1, j] + 1, d[i, j - 1] + 1, d[i - 1, j - 1] + cost)
      d[i, j] <- min(options)
      if (i > 2 && j > 2 && substring(a, i, i) == substring(b, j - 1, j - 1) && substring(a, i - 1, i - 1) == substring(b, j, j)) {
        options <- c(d[i, j], d[i - 2, j - 2] + 1)
        d[i, j] <- min(options)
      }
    }
  }
  return(d[n1 + 1, n2 + 1])
}

language_distances <- function() {
  # plot histogram
  # mean and variance
  words <- scan('data.txt', what='', sep='\n')
  n <- length(words)
  distances <- matrix(nrow = n + 1, ncol = n + 1)
  distances[1, 1] = '/'
  for (i in 2:(n+1)) {
    distances[1, i] <- words[i - 1]
  }
  for (i in 2:(n+1)) {
    distances[i, 1] <- words[i - 1]
  }
  for (i in 2:(n+1)) {
    for (j in 2:(n+1)) {
      distances[i, j] <- dldistance(words[i - 1], words[j - 1])
    }
  }
  write.table(distances, file="results/distances.txt", row.names=FALSE, col.names=FALSE, sep = '\t\t')
  return(distances)
}

main <- function() {
  distances <- language_distances()
  n = dim(distances)[1] - 1
  elements <- array(0L, dim = c(n * (n - 1)))
  index = 1
  for (i in 2:n) {
    for (j in 2:n) {
      value <- strtoi(distances[i, j], base = 0L)
      elements[index] <- value
      index <- index + 1
    }
  }
  png('images/histogram.png', width = 600, height = 600)
  hist(elements, xlab = 'Distance')
  print(c('Distancia promedio: ', mean(elements)))
  print(c('Varianza', var(elements)))
  dev.off()
}

main()
