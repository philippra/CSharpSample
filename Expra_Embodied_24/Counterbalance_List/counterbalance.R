set.seed(9999)


generate_counterbalance_list <- function(){
    id <- seq(1:40)

  counterbalance <- rep(0, times = 40)
  
  cond_df <- as.data.frame(matrix(c(1, 2, 3, 4, 5, 6, 7, 8), ncol=1))
  colnames(cond_df) <- c("Counterbalance")
  cond_df
  
  counterbalance_df <- as.data.frame(id)
  counterbalance_df$Counterbalance <- c(1, 2, 3, 4, 5, 6, 7, 8)
  
  counter <- 1
  for (i in 1:(length(id) / 8)){
    
    rand_conds <- sample(c(1:8))
    
    for (r in rand_conds){
      counterbalance_df$Counterbalance[counter] <- cond_df[r, 1]
      counter <- counter + 1
    }
    
    
  }
  
  return(counterbalance_df)
  
}

counterbalance_df <- generate_counterbalance_list()

counterbalance_df

write.table(counterbalance_df, "counterbalance_expra.csv", sep = ",",
            row.names = F)