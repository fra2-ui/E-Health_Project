# EHealth Project: Advanced Clustering & Personas Analysis

This notebook integrates multiple analytical approaches to analyze the eHealth questionnaire dataset. The goal is to explore the data structure and determine the optimal number of clusters to generate meaningful Personas for Game Design.

## Workflow Overview

0.  **Environment Setup & Dependencies**:
    - **Version Compatibility**: This analysis has been rigorously tested and validated **exclusively on Python 3.11.14**. Please note that utilizing alternative interpreter versions may lead to dependency conflicts or execution variances.
    - **Dependencies**: To ensure reproducibility and enable the advanced statistical methods required for this analysis, specific Python libraries must be installed.

1.  **Data Loading & Cleaning**: 
    - **Standardization**: Normalizing column names for consistency across the dataset.
    - **Cleaning**: Identifying duplicates and handling noise (e.g., converting "Prefer not to say" to missing values).
    - **Structural Validation**: Preliminary inspection of the raw correlation matrix to confirm item-scale consistency before aggregation.

2.  **Advanced Imputation & Feature Engineering**:
    - **Imputation**: Hybrid approach using Random Forest (Gender), Median/Mode (Demographics), and Row-wise Mean (Questionnaire scales > 60% valid).
    - **Scoring**: Calculation of psychometric scores (AUDIT, DAST, IAT, etc.) to be used as primary features.

3.  **Exploratory Data Analysis (EDA)**:
    - **Univariate**: Histograms/PDFs (Numerical) and Bar Charts (Categorical) to check distributions.
    - **Outlier Detection**: Global Boxplots to identify anomalies.
    - **Bivariate**: Stacked Bar Charts (Cat vs Cat) and Boxplots (Cat vs Num) to visualize relationships.
    - **Multivariate**: Correlation Heatmaps and Multiple Regression to understand variable interdependencies.
    - **Statistical Pre-Check**: Normality checks (Lilliefors/K-S) to determine appropriate hypothesis testing strategies.

4.  **Clustering Analysis**:
    - Comparison of 4 preprocessing methods: **Z-Score** (Baseline), **Gower Distance** (Mixed Data), **FAMD** (Mixed Factor Analysis), and **PCA** (Numerical Factor Analysis).
    - Algorithms: K-Medoids (PAM) vs Hierarchical Clustering.
    - **Validation**: Selection based on a Quality Score combining **Silhouette** (Geometry) and **% Statistical Difference** (Clinical Relevance), supplemented by **Inertia** analysis for the Elbow Method.

5.  **Model Selection & Profiling**:
    - **Rigorous Statistical Validation**: Adaptive hypothesis testing using **ANOVA/Kruskal-Wallis** (numerical) and **Chi-Square/Fisher's Exact** (categorical), reinforced by **Pairwise Post-Hoc tests** with Bonferroni correction.
    - **Personas Generation**: Automatic derivation of "Archetypes" via descriptive tagging based on significant deviations from global means.
    - **3D Visualization**: Projection of the identified Personas into a 3D PCA space to verify geometric separation.

6.  **Final Report**:
    - **Cluster Interpretation**: Identification of four distinct behavioral archetypes within the population based on rigorous statistical profiling and model selection.
    - **Actionable Insights**: Derivation of specific guidelines for targeted intervention and game design customization based on the identified Personas.
