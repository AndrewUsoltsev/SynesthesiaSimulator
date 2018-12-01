import numpy as np
import matplotlib.pyplot as plt
import pylab
import sys
import time


from sklearn import decomposition
from sklearn import datasets
from mpl_toolkits.mplot3d import Axes3D
from sklearn.cluster import KMeans 
from sklearn.cluster import AffinityPropagation
from scipy.spatial.distance import cdist
from scipy.optimize import minimize
def CollectData(filename):
    str_file = []
    x1 = []
    x2 = []
    x3 = []
    X = []
    with open(filename, 'r') as f:
    #with open("aaa.txt", 'r') as f:
        for line in f:
            str_file.append(line)
    for i in range(0,len(str_file)):
        tmp = [int(n) for n in str_file[i].split()]
        if tmp: 
            x1.append(tmp[0])
            x2.append(tmp[1])
            x3.append(tmp[2])
            X.append([tmp[0], tmp[1], tmp[2]])    
    return X
        
def D(J, k):
    prevVal = 1.4*J[0]
    nextVal = 0
    if (k > 0):
        prevVal = J[k-1]
        
    if (k >= (len(J))):
        nextVal = 0
    else:
        nextVal = J[k+1]
    if (abs(prevVal - J[k]) < 1E-5):
        return abs(J[k]-nextVal) / 1E-5
    else:
        return abs(J[k]-nextVal)/abs(prevVal - J[k])

if __name__ == "__main__":
    colorCount = 8
    x = CollectData(sys.argv[1])
    if len(x) <= colorCount * 2:
        colorCount = 4
    X = np.array(x)
    inertia = []
    centroids = []
    minDist = 1E+8
    for k in range(1, colorCount):
        kmeans = KMeans(n_clusters=k, random_state=1).fit(X)
        inertia.append(np.sqrt(kmeans.inertia_))
        centroids.append(np.array(kmeans.cluster_centers_))
    arrayD = []
    for k in range(len(inertia)-1):
        arrayD.append(D(inertia,k))
    minCentroids = centroids[np.argmin(arrayD)]
    for i in range(len(minCentroids)):
        print(int(minCentroids[i][0]), int(minCentroids[i][1]), int(minCentroids[i][2]), sep = ' ')

