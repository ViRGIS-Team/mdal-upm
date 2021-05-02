#!/bin/bash

while getopts "p:i:d:t:" opt
do
   case "$opt" in
      p ) package="$OPTARG" ;;
      d ) destination="$OPTARG" ;;
      i ) install="$OPTARG" ;;
      t ) test="$OPTARG" ;;
   esac
done

conda install -c conda-forge --prefix $destination --copy --mkdir $install -y --no-deps

